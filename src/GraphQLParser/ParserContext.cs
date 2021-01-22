using System.Collections.Generic;
using GraphQLParser.AST;
using GraphQLParser.Exceptions;

namespace GraphQLParser
{
    // WARNING: mutable struct, pass it by reference to those methods that will change it
    internal struct ParserContext
    {
        private delegate TResult ParseCallback<out TResult>(ref ParserContext context);

        private readonly ROM _source;
        private readonly IgnoreOptions _ignoreOptions;
        private GraphQLComment? _currentComment;
        private List<GraphQLComment>? _unattachedComments;
        private Token _currentToken;
        private Token _prevToken;
        private GraphQLDocument? _document;

        public ParserContext(ROM source, ParserOptions options)
        {
            _document = null;
            _currentComment = null;
            _unattachedComments = null;
            _source = source;
            _ignoreOptions = options.Ignore;

            _currentToken = Lexer.Lex(source);
            _prevToken = new Token
            (
                TokenKind.UNKNOWN,
                default,
                0,
                0
            );
        }

        public GraphQLComment? GetComment()
        {
            var ret = _currentComment;
            _currentComment = null;
            return ret;
        }

        public GraphQLDocument Parse() => ParseDocument();

        private void Advance()
        {
            // We should not advance further if we have already reached the EOF.
            if (_currentToken.Kind != TokenKind.EOF)
            {
                _prevToken = _currentToken;
                _currentToken = Lexer.Lex(_source, _currentToken.End);
            }
        }

        private GraphQLType AdvanceThroughColonAndParseType()
        {
            Expect(TokenKind.COLON);
            return ParseType();
        }

        private List<T>? Any<T>(TokenKind open, ParseCallback<T> next, TokenKind close)
            where T : ASTNode
        {
            Expect(open);

            ParseComment();

            List<T>? nodes = null;
            while (!Skip(close))
                (nodes ??= new List<T>()).Add(next(ref this));

            return nodes;
        }

        private GraphQLFieldSelection CreateFieldSelection(int start, GraphQLName name, GraphQLName? alias, GraphQLComment? comment)
        {
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLFieldSelection
                {
                    Alias = alias,
                    Name = name,
                    Arguments = ParseArguments(),
                    Directives = ParseDirectives(),
                    SelectionSet = Peek(TokenKind.BRACE_L) ? ParseSelectionSet() : null,
                }
                : new GraphQLFieldSelectionFull
                {
                    Comment = comment,
                    Alias = alias,
                    Name = name,
                    Arguments = ParseArguments(),
                    Directives = ParseDirectives(),
                    SelectionSet = Peek(TokenKind.BRACE_L) ? ParseSelectionSet() : null,
                    Location = GetLocation(start)
                };
        }

        private ASTNode CreateGraphQLFragmentSpread(int start, GraphQLComment? comment)
        {
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLFragmentSpread
                {
                    Name = ParseFragmentName(),
                    Directives = ParseDirectives(),
                }
                : new GraphQLFragmentSpreadFull
                {
                    Comment = comment,
                    Name = ParseFragmentName(),
                    Directives = ParseDirectives(),
                    Location = GetLocation(start)
                };
        }

        private ASTNode CreateInlineFragment(int start, GraphQLComment? comment)
        {
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLInlineFragment
                {
                    TypeCondition = GetTypeCondition(),
                    Directives = ParseDirectives(),
                    SelectionSet = ParseSelectionSet(),
                }
                : new GraphQLInlineFragmentFull
                {
                    Comment = comment,
                    TypeCondition = GetTypeCondition(),
                    Directives = ParseDirectives(),
                    SelectionSet = ParseSelectionSet(),
                    Location = GetLocation(start)
                };
        }

        private ASTNode CreateOperationDefinition(int start, OperationType operation, GraphQLName? name)
        {
            var comment = GetComment();
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLOperationDefinition
                {
                    Operation = operation,
                    Name = name,
                    VariableDefinitions = ParseVariableDefinitions(),
                    Directives = ParseDirectives(),
                    SelectionSet = ParseSelectionSet(),
                }
                : new GraphQLOperationDefinitionFull
                {
                    Comment = comment,
                    Operation = operation,
                    Name = name,
                    VariableDefinitions = ParseVariableDefinitions(),
                    Directives = ParseDirectives(),
                    SelectionSet = ParseSelectionSet(),
                    Location = GetLocation(start)
                };
        }

        private ASTNode CreateOperationDefinition(int start)
        {
            var comment = GetComment();
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLOperationDefinition
                {
                    Operation = OperationType.Query,
                    SelectionSet = ParseSelectionSet(),
                }
                : new GraphQLOperationDefinitionFull
                {
                    Comment = comment,
                    Operation = OperationType.Query,
                    SelectionSet = ParseSelectionSet(),
                    Location = GetLocation(start)
                };
        }

        private void Expect(TokenKind kind)
        {
            if (_currentToken.Kind == kind)
            {
                Advance();
            }
            else
            {
                Throw_From_Expect(kind);
            }
        }

        private void Throw_From_Expect(TokenKind kind)
        {
            throw new GraphQLSyntaxErrorException($"Expected {Token.GetTokenKindDescription(kind)}, found {_currentToken}", _source, _currentToken.Start);
        }

        private GraphQLValue ExpectColonAndParseValueLiteral(bool isConstant)
        {
            Expect(TokenKind.COLON);
            return ParseValueLiteral(isConstant);
        }

        private void ExpectKeyword(string keyword)
        {
            if (_currentToken.Kind == TokenKind.NAME && _currentToken.Value == keyword)
                Advance();
            else
                Throw_From_ExpectKeyword(keyword);
        }

        private void Throw_From_ExpectKeyword(string keyword)
        {
            throw new GraphQLSyntaxErrorException($"Expected \"{keyword}\", found Name \"{_currentToken.Value}\"", _source, _currentToken.Start);
        }

        private GraphQLNamedType ExpectOnKeywordAndParseNamedType()
        {
            ExpectKeyword("on");
            return ParseNamedType();
        }

        private GraphQLValue? GetDefaultConstantValue()
        {
            GraphQLValue? defaultValue = null;
            if (Skip(TokenKind.EQUALS))
            {
                defaultValue = ParseConstantValue();
            }

            return defaultValue;
        }

        private GraphQLLocation GetLocation(int start)
        {
            return new GraphQLLocation
            (
                start,
                _prevToken.End
            );
        }

        private GraphQLName? GetName() => Peek(TokenKind.NAME) ? ParseName() : null;

        private GraphQLNamedType? GetTypeCondition()
        {
            GraphQLNamedType? typeCondition = null;
            if (_currentToken.Value == "on")
            {
                Advance();
                typeCondition = ParseNamedType();
            }

            return typeCondition;
        }

        private List<T> Many<T>(TokenKind open, ParseCallback<T> next, TokenKind close)
        {
            Expect(open);

            ParseComment();

            var nodes = new List<T> { next(ref this) };
            while (!Skip(close))
                nodes.Add(next(ref this));

            return nodes;
        }

        private GraphQLArgument ParseArgument()
        {
            var comment = GetComment();
            int start = _currentToken.Start;

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLArgument
                {
                    Name = ParseName(),
                    Value = ExpectColonAndParseValueLiteral(false),
                }
                : new GraphQLArgumentFull
                {
                    Comment = comment,
                    Name = ParseName(),
                    Value = ExpectColonAndParseValueLiteral(false),
                    Location = GetLocation(start)
                };
        }

        private List<GraphQLInputValueDefinition>? ParseArgumentDefs()
        {
            return Peek(TokenKind.PAREN_L)
                ? Many(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseInputValueDef(), TokenKind.PAREN_R)
                : null;
        }

        private List<GraphQLArgument>? ParseArguments()
        {
            return Peek(TokenKind.PAREN_L) ?
                Many(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseArgument(), TokenKind.PAREN_R) :
                null;
        }

        private GraphQLValue ParseBooleanValue(Token token)
        {
            Advance();
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLScalarValue(ASTNodeKind.BooleanValue)
                {
                    Value = token.Value,
                }
                : new GraphQLScalarValueFull(ASTNodeKind.BooleanValue)
                {
                    Value = token.Value,
                    Location = GetLocation(token.Start)
                };
        }

        private GraphQLValue ParseConstantValue() => ParseValueLiteral(true);

        private ASTNode ParseDefinition()
        {
            ParseComment();

            if (Peek(TokenKind.BRACE_L))
            {
                return ParseOperationDefinition();
            }

            if (Peek(TokenKind.NAME))
            {
                ASTNode? definition;
                if ((definition = ParseNamedDefinition()) != null)
                    return definition;
            }

            return Throw_From_ParseDefinition();
        }

        private ASTNode Throw_From_ParseDefinition()
        {
            throw new GraphQLSyntaxErrorException($"Unexpected {_currentToken}", _source, _currentToken.Start);
        }

        private List<ASTNode> ParseDefinitionsIfNotEOF()
        {
            var result = new List<ASTNode>();

            if (_currentToken.Kind != TokenKind.EOF)
            {
                do
                {
                    result.Add(ParseDefinition());
                }
                while (!Skip(TokenKind.EOF));
            }

            return result;
        }

        private GraphQLComment? ParseComment()
        {
            if (_ignoreOptions != IgnoreOptions.None)
            {
                while (Peek(TokenKind.COMMENT))
                {
                    Advance();
                }
                return null;
            }

            if (!Peek(TokenKind.COMMENT))
            {
                return null;
            }

            var text = new List<ROM>();
            int start = _currentToken.Start;
            int end;

            do
            {
                text.Add(_currentToken.Value);
                end = _currentToken.End;
                Advance();
            }
            while (_currentToken.Kind == TokenKind.COMMENT);

            var comment = new GraphQLComment
            {
                Location = new GraphQLLocation
                (
                    start,
                    end
                )
            };

            if (text.Count == 1)
            {
                comment.Text = text[0];
            }
            else if (text.Count > 1)
            {
                var (owner, result) = text.Concat();
                comment.Text = result;
                (_document!.RentedMemoryTracker ??= new List<(System.Buffers.IMemoryOwner<char>, ASTNode)>()).Add((owner, comment));
            }

            SetCurrentComment(comment);

            return comment;
        }

        private void SetCurrentComment(GraphQLComment? comment)
        {
            if (_currentComment != null)
                (_unattachedComments ??= new List<GraphQLComment>()).Add(_currentComment);

            _currentComment = comment;
        }

        private GraphQLDirective ParseDirective()
        {
            int start = _currentToken.Start;
            Expect(TokenKind.AT);
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLDirective
                {
                    Name = ParseName(),
                    Arguments = ParseArguments(),
                }
                : new GraphQLDirectiveFull
                {
                    Name = ParseName(),
                    Arguments = ParseArguments(),
                    Location = GetLocation(start)
                };
        }

        /// <summary>
        /// http://spec.graphql.org/draft/#DirectiveDefinition
        /// DirectiveDefinition:
        ///     Description(opt) directive @ Name ArgumentsDefinition(opt) repeatable(opt) on DirectiveLocations
        /// </summary>
        /// <returns></returns>
        private GraphQLDirectiveDefinition ParseDirectiveDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("directive");
            Expect(TokenKind.AT);

            var name = ParseName();
            var args = ParseArgumentDefs();
            bool repeatable = ParseRepeatable();

            ExpectKeyword("on");
            var locations = ParseDirectiveLocations();

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLDirectiveDefinition
                {
                    Name = name,
                    Repeatable = repeatable,
                    Arguments = args,
                    Locations = locations,
                }
                : new GraphQLDirectiveDefinitionFull
                {
                    Comment = comment,
                    Name = name,
                    Repeatable = repeatable,
                    Arguments = args,
                    Locations = locations,
                    Location = GetLocation(start)
                };
        }

        private bool ParseRepeatable()
        {
            if (Peek(TokenKind.NAME))
            {
                if (_currentToken.Value == "on")
                    return false;

                if (_currentToken.Value == "repeatable")
                {
                    Advance();
                    return true;
                }

                Throw_From_ParseRepeatable();
            }

            return false;
        }

        private void Throw_From_ParseRepeatable()
        {
            throw new GraphQLSyntaxErrorException($"Unexpected {_currentToken}", _source, _currentToken.Start);
        }

        private List<GraphQLName> ParseDirectiveLocations()
        {
            var locations = new List<GraphQLName>();

            // Directive locations may be defined with an optional leading | character
            // to aid formatting when representing a longer list of possible locations
            Skip(TokenKind.PIPE);

            do
            {
                locations.Add(ParseName());
            }
            while (Skip(TokenKind.PIPE));

            return locations;
        }

        private List<GraphQLDirective>? ParseDirectives()
        {
            List<GraphQLDirective>? directives = null;
            while (Peek(TokenKind.AT))
                (directives ??= new List<GraphQLDirective>()).Add(ParseDirective());

            return directives;
        }

        private GraphQLDocument ParseDocument()
        {
            _document = _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations ? new GraphQLDocument() : new GraphQLDocumentFull();

            int start = _currentToken.Start;
            var definitions = ParseDefinitionsIfNotEOF();

            SetCurrentComment(null);

            if (_ignoreOptions != IgnoreOptions.IgnoreCommentsAndLocations)
            {
                _document.Location = new GraphQLLocation
                (
                    start,
                    // Formally, to denote the end of the document, it is better to use _prevToken.End,
                    // since _prevToken represents some real meaningful token; _currentToken here is always EOF.
                    // EOF is a technical token with length = 0, _prevToken.End and _currentToken.End have the same value here.
                    _prevToken.End // equals to _currentToken.End (EOF) 
                );
            }
            _document.Definitions = definitions;
            _document.UnattachedComments = _unattachedComments;
            return _document;
        }

        private GraphQLEnumTypeDefinition ParseEnumTypeDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("enum");

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLEnumTypeDefinition
                {
                    Name = ParseName(),
                    Directives = ParseDirectives(),
                    Values = Many(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseEnumValueDefinition(), TokenKind.BRACE_R),
                }
                : new GraphQLEnumTypeDefinitionFull
                {
                    Comment = comment,
                    Name = ParseName(),
                    Directives = ParseDirectives(),
                    Values = Many(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseEnumValueDefinition(), TokenKind.BRACE_R),
                    Location = GetLocation(start)
                };
        }

        private GraphQLValue ParseEnumValue(Token token)
        {
            Advance();
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLScalarValue(ASTNodeKind.EnumValue)
                {
                    Value = token.Value,
                }
                : new GraphQLScalarValueFull(ASTNodeKind.EnumValue)
                {
                    Value = token.Value,
                    Location = GetLocation(token.Start)
                };
        }

        private GraphQLEnumValueDefinition ParseEnumValueDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLEnumValueDefinition
                {
                    Name = ParseName(),
                    Directives = ParseDirectives(),
                }
                : new GraphQLEnumValueDefinitionFull
                {
                    Comment = comment,
                    Name = ParseName(),
                    Directives = ParseDirectives(),
                    Location = GetLocation(start)
                };
        }

        private GraphQLFieldDefinition ParseFieldDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            var name = ParseName();
            var args = ParseArgumentDefs();
            Expect(TokenKind.COLON);

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLFieldDefinition
                {
                    Name = name,
                    Arguments = args,
                    Type = ParseType(),
                    Directives = ParseDirectives(),
                }
                : new GraphQLFieldDefinitionFull
                {
                    Comment = comment,
                    Name = name,
                    Arguments = args,
                    Type = ParseType(),
                    Directives = ParseDirectives(),
                    Location = GetLocation(start)
                };
        }

        private GraphQLFieldSelection ParseFieldSelection()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            var nameOrAlias = ParseName();
            GraphQLName name;
            GraphQLName? alias;

            if (Skip(TokenKind.COLON))
            {
                name = ParseName();
                alias = nameOrAlias;
            }
            else
            {
                alias = null;
                name = nameOrAlias;
            }

            return CreateFieldSelection(start, name, alias, comment);
        }

        private GraphQLValue ParseFloat(/*bool isConstant*/)
        {
            var token = _currentToken;
            Advance();
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLScalarValue(ASTNodeKind.FloatValue)
                {
                    Value = token.Value,
                }
                : new GraphQLScalarValueFull(ASTNodeKind.FloatValue)
                {
                    Value = token.Value,
                    Location = GetLocation(token.Start)
                };
        }

        private ASTNode ParseFragment()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            Expect(TokenKind.SPREAD);

            return Peek(TokenKind.NAME) && _currentToken.Value != "on"
                ? CreateGraphQLFragmentSpread(start, comment)
                : CreateInlineFragment(start, comment);
        }

        private GraphQLFragmentDefinition ParseFragmentDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("fragment");

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations 
                ? new GraphQLFragmentDefinition
                {
                    Name = ParseFragmentName(),
                    TypeCondition = ExpectOnKeywordAndParseNamedType(),
                    Directives = ParseDirectives(),
                    SelectionSet = ParseSelectionSet(),
                }
                : new GraphQLFragmentDefinitionFull
                {
                    Comment = comment,
                    Name = ParseFragmentName(),
                    TypeCondition = ExpectOnKeywordAndParseNamedType(),
                    Directives = ParseDirectives(),
                    SelectionSet = ParseSelectionSet(),
                    Location = GetLocation(start)
                };
        }

        private GraphQLName ParseFragmentName()
        {
            if (_currentToken.Value == "on")
            {
                Throw_From_ParseFragmentName();
            }

            return ParseName();
        }

        private void Throw_From_ParseFragmentName()
        {
            throw new GraphQLSyntaxErrorException($"Unexpected {_currentToken}", _source, _currentToken.Start);
        }

        private List<GraphQLNamedType>? ParseImplementsInterfaces()
        {
            List<GraphQLNamedType>? types = null;
            if (_currentToken.Value == "implements")
            {
                types = new List<GraphQLNamedType>();
                Advance();

                do
                {
                    types.Add(ParseNamedType());
                }
                while (Peek(TokenKind.NAME));
            }

            return types;
        }

        private GraphQLInputObjectTypeDefinition ParseInputObjectTypeDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("input");

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations 
                ? new GraphQLInputObjectTypeDefinition
                {
                    Name = ParseName(),
                    Directives = ParseDirectives(),
                    Fields = Any(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseInputValueDef(), TokenKind.BRACE_R),
                }
                : new GraphQLInputObjectTypeDefinitionFull
                {
                    Comment = comment,
                    Name = ParseName(),
                    Directives = ParseDirectives(),
                    Fields = Any(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseInputValueDef(), TokenKind.BRACE_R),
                    Location = GetLocation(start)
                };
        }

        private GraphQLInputValueDefinition ParseInputValueDef()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            var name = ParseName();
            Expect(TokenKind.COLON);

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLInputValueDefinition
                {
                    Name = name,
                    Type = ParseType(),
                    DefaultValue = GetDefaultConstantValue(),
                    Directives = ParseDirectives(),
                }
                : new GraphQLInputValueDefinitionFull
                {
                    Comment = comment,
                    Name = name,
                    Type = ParseType(),
                    DefaultValue = GetDefaultConstantValue(),
                    Directives = ParseDirectives(),
                    Location = GetLocation(start)
                };
        }

        private GraphQLValue ParseInt(/*bool isConstant*/)
        {
            var token = _currentToken;
            Advance();

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLScalarValue(ASTNodeKind.IntValue)
                {
                    Value = token.Value,
                }
                : new GraphQLScalarValueFull(ASTNodeKind.IntValue)
                {
                    Value = token.Value,
                    Location = GetLocation(token.Start)
                };
        }

        private GraphQLInterfaceTypeDefinition ParseInterfaceTypeDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("interface");

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLInterfaceTypeDefinition
                {
                    Name = ParseName(),
                    Directives = ParseDirectives(),
                    Fields = Any(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseFieldDefinition(), TokenKind.BRACE_R),
                }
                : new GraphQLInterfaceTypeDefinitionFull
                {
                    Comment = comment,
                    Name = ParseName(),
                    Directives = ParseDirectives(),
                    Fields = Any(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseFieldDefinition(), TokenKind.BRACE_R),
                    Location = GetLocation(start)
                };
        }

        private GraphQLValue ParseList(bool isConstant)
        {
            int start = _currentToken.Start;
            // the compiler caches these delegates in the generated code
            ParseCallback<GraphQLValue> constant = (ref ParserContext context) => context.ParseConstantValue();
            ParseCallback<GraphQLValue> value = (ref ParserContext context) => context.ParseValueValue();

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLListValue(ASTNodeKind.ListValue)
                {
                    Values = Any(TokenKind.BRACKET_L, isConstant ? constant : value, TokenKind.BRACKET_R),
                    AstValue = _source.Slice(start, _currentToken.End - start - 1)
                }
                : new GraphQLListValueFull(ASTNodeKind.ListValue)
                {
                    Values = Any(TokenKind.BRACKET_L, isConstant ? constant : value, TokenKind.BRACKET_R),
                    Location = GetLocation(start),
                    AstValue = _source.Slice(start, _currentToken.End - start - 1)
                };
        }

        private GraphQLName ParseName()
        {
            int start = _currentToken.Start;
            var value = _currentToken.Value;

            Expect(TokenKind.NAME);

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLName
                {
                    Value = value
                }
                : new GraphQLNameFull
                {
                    Location = GetLocation(start),
                    Value = value
                };
        }

        private ASTNode? ParseNamedDefinition()
        {
            var value = _currentToken.Value;

            if (value == "query")
                return ParseOperationDefinition();

            if (value == "mutation")
                return ParseOperationDefinition();

            if (value == "subscription")
                return ParseOperationDefinition();

            if (value == "fragment")
                return ParseFragmentDefinition();

            if (value == "schema")
                return ParseSchemaDefinition();

            if (value == "scalar")
                return ParseScalarTypeDefinition();

            if (value == "type")
                return ParseObjectTypeDefinition();

            if (value == "interface")
                return ParseInterfaceTypeDefinition();

            if (value == "union")
                return ParseUnionTypeDefinition();

            if (value == "enum")
                return ParseEnumTypeDefinition();

            if (value == "input")
                return ParseInputObjectTypeDefinition();

            if (value == "extend")
                return ParseTypeExtensionDefinition();

            if (value == "directive")
                return ParseDirectiveDefinition();

            return null;
        }

        private GraphQLNamedType ParseNamedType()
        {
            int start = _currentToken.Start;
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLNamedType
                {
                    Name = ParseName(),
                }
                : new GraphQLNamedTypeFull
                {
                    Name = ParseName(),
                    Location = GetLocation(start)
                };
        }

        private GraphQLValue ParseNameValue(/*bool isConstant*/)
        {
            var token = _currentToken;

            if (token.Value == "true" || token.Value == "false")
            {
                return ParseBooleanValue(token);
            }
            else if (!token.Value.IsEmpty)
            {
                return token.Value == "null"
                    ? ParseNullValue(token)
                    : ParseEnumValue(token);
            }

            return Throw_From_ParseNameValue();
        }

        private GraphQLValue Throw_From_ParseNameValue()
        {
            throw new GraphQLSyntaxErrorException($"Unexpected {_currentToken}", _source, _currentToken.Start);
        }

        private GraphQLValue ParseObject(bool isConstant)
        {
            var comment = GetComment();
            int start = _currentToken.Start;

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLObjectValue
                {
                    Fields = ParseObjectFields(isConstant),
                }
                : new GraphQLObjectValueFull
                {
                    Comment = comment,
                    Fields = ParseObjectFields(isConstant),
                    Location = GetLocation(start)
                };
        }

        private GraphQLValue ParseNullValue(Token token)
        {
            Advance();
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLScalarValue(ASTNodeKind.NullValue)
                {
                    Value = token.Value,
                }
                : new GraphQLScalarValueFull(ASTNodeKind.NullValue)
                {
                    Value = token.Value,
                    Location = GetLocation(token.Start)
                };
        }

        private GraphQLObjectField ParseObjectField(bool isConstant)
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLObjectField
                {
                    Name = ParseName(),
                    Value = ExpectColonAndParseValueLiteral(isConstant),
                }
                : new GraphQLObjectFieldFull
                {
                    Comment = comment,
                    Name = ParseName(),
                    Value = ExpectColonAndParseValueLiteral(isConstant),
                    Location = GetLocation(start)
                };
        }

        private List<GraphQLObjectField> ParseObjectFields(bool isConstant)
        {
            var fields = new List<GraphQLObjectField>();

            Expect(TokenKind.BRACE_L);
            while (!Skip(TokenKind.BRACE_R))
                fields.Add(ParseObjectField(isConstant));

            return fields;
        }

        private GraphQLObjectTypeDefinition ParseObjectTypeDefinition()
        {
            var comment = GetComment();

            int start = _currentToken.Start;
            ExpectKeyword("type");

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLObjectTypeDefinition
                {
                    Name = ParseName(),
                    Interfaces = ParseImplementsInterfaces(),
                    Directives = ParseDirectives(),
                    Fields = Any(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseFieldDefinition(), TokenKind.BRACE_R),
                }
                : new GraphQLObjectTypeDefinitionFull
                {
                    Comment = comment,
                    Name = ParseName(),
                    Interfaces = ParseImplementsInterfaces(),
                    Directives = ParseDirectives(),
                    Fields = Any(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseFieldDefinition(), TokenKind.BRACE_R),
                    Location = GetLocation(start)
                };
        }

        private ASTNode ParseOperationDefinition()
        {
            int start = _currentToken.Start;

            return Peek(TokenKind.BRACE_L)
                ? CreateOperationDefinition(start)
                : CreateOperationDefinition(start, ParseOperationType(), GetName());
        }

        private OperationType ParseOperationType()
        {
            var token = _currentToken;
            Expect(TokenKind.NAME);

            if (token.Value == "mutation")
                return OperationType.Mutation;

            if (token.Value == "subscription")
                return OperationType.Subscription;

            return OperationType.Query;
        }

        private GraphQLOperationTypeDefinition ParseOperationTypeDefinition()
        {
            int start = _currentToken.Start;
            var operation = ParseOperationType();
            Expect(TokenKind.COLON);
            var type = ParseNamedType();

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLOperationTypeDefinition
                {
                    Operation = operation,
                    Type = type,
                }
                : new GraphQLOperationTypeDefinitionFull
                {
                    Operation = operation,
                    Type = type,
                    Location = GetLocation(start)
                };
        }

        private GraphQLScalarTypeDefinition ParseScalarTypeDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("scalar");
            var name = ParseName();
            var directives = ParseDirectives();

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLScalarTypeDefinition
                {
                    Name = name,
                    Directives = directives,
                }
                : new GraphQLScalarTypeDefinitionFull
                {
                    Comment = comment,
                    Name = name,
                    Directives = directives,
                    Location = GetLocation(start)
                };
        }

        private GraphQLSchemaDefinition ParseSchemaDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("schema");
            var directives = ParseDirectives();
            var operationTypes = Many(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseOperationTypeDefinition(), TokenKind.BRACE_R);

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLSchemaDefinition
                {
                    Directives = directives,
                    OperationTypes = operationTypes,
                }
                : new GraphQLSchemaDefinitionFull
                {
                    Comment = comment,
                    Directives = directives,
                    OperationTypes = operationTypes,
                    Location = GetLocation(start)
                };
        }

        private ASTNode ParseSelection()
        {
            return Peek(TokenKind.SPREAD) ?
                ParseFragment() :
                ParseFieldSelection();
        }

        private GraphQLSelectionSet ParseSelectionSet()
        {
            int start = _currentToken.Start;
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLSelectionSet
                {
                    Selections = Many(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseSelection(), TokenKind.BRACE_R),
                }
                : new GraphQLSelectionSetFull
                {
                    Selections = Many(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseSelection(), TokenKind.BRACE_R),
                    Location = GetLocation(start)
                };
        }

        private GraphQLValue ParseString(/*bool isConstant*/)
        {
            var token = _currentToken;
            Advance();
            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLScalarValue(ASTNodeKind.StringValue)
                {
                    Value = token.Value,
                }
                : new GraphQLScalarValueFull(ASTNodeKind.StringValue)
                {
                    Value = token.Value,
                    Location = GetLocation(token.Start)
                };
        }

        private GraphQLType ParseType()
        {
            GraphQLType type;
            int start = _currentToken.Start;
            if (Skip(TokenKind.BRACKET_L))
            {
                type = ParseType();
                Expect(TokenKind.BRACKET_R);
                type = _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                    ? new GraphQLListType
                    {
                        Type = type,
                    }
                    : new GraphQLListTypeFull
                    {
                        Type = type,
                        Location = GetLocation(start)
                    };
            }
            else
            {
                type = ParseNamedType();
            }

            return Skip(TokenKind.BANG)
                ? (_ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLNonNullType
                {
                    Type = type,
                }
                : new GraphQLNonNullTypeFull
                {
                    Type = type,
                    Location = GetLocation(start)
                })
                : type;
        }

        private GraphQLTypeExtensionDefinition ParseTypeExtensionDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("extend");
            var definition = ParseObjectTypeDefinition();

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLTypeExtensionDefinition
                {
                    Name = definition.Name,
                    Definition = definition,
                }
                : new GraphQLTypeExtensionDefinitionFull
                {
                    Comment = comment,
                    Name = definition.Name,
                    Definition = definition,
                    Location = GetLocation(start)
                };
        }

        private List<GraphQLNamedType> ParseUnionMembers()
        {
            var members = new List<GraphQLNamedType>();

            // Union members may be defined with an optional leading | character
            // to aid formatting when representing a longer list of possible types
            Skip(TokenKind.PIPE);

            do
            {
                members.Add(ParseNamedType());
            }
            while (Skip(TokenKind.PIPE));

            return members;
        }

        private GraphQLUnionTypeDefinition ParseUnionTypeDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;
            ExpectKeyword("union");
            var name = ParseName();
            var directives = ParseDirectives();
            Expect(TokenKind.EQUALS);
            var types = ParseUnionMembers();

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLUnionTypeDefinition
                {
                    Name = name,
                    Directives = directives,
                    Types = types,
                }
                : new GraphQLUnionTypeDefinitionFull
                {
                    Comment = comment,
                    Name = name,
                    Directives = directives,
                    Types = types,
                    Location = GetLocation(start)
                };
        }

        private GraphQLValue ParseValueLiteral(bool isConstant) => _currentToken.Kind switch
        {
            TokenKind.BRACKET_L => ParseList(isConstant),
            TokenKind.BRACE_L => ParseObject(isConstant),
            TokenKind.INT => ParseInt(/*isConstant*/),
            TokenKind.FLOAT => ParseFloat(/*isConstant*/),
            TokenKind.STRING => ParseString(/*isConstant*/),
            TokenKind.NAME => ParseNameValue(/*isConstant*/),
            TokenKind.DOLLAR when !isConstant => ParseVariable(),
            _ => Throw_From_ParseValueLiteral()
        };

        private GraphQLValue Throw_From_ParseValueLiteral()
        {
            throw new GraphQLSyntaxErrorException($"Unexpected {_currentToken}", _source, _currentToken.Start);
        }

        private GraphQLValue ParseValueValue() => ParseValueLiteral(false);

        private GraphQLVariable ParseVariable()
        {
            int start = _currentToken.Start;
            Expect(TokenKind.DOLLAR);

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLVariable
                {
                    Name = GetName(),
                }
                : new GraphQLVariableFull
                {
                    Name = GetName(),
                    Location = GetLocation(start)
                };
        }

        private GraphQLVariableDefinition ParseVariableDefinition()
        {
            var comment = GetComment();
            int start = _currentToken.Start;

            return _ignoreOptions == IgnoreOptions.IgnoreCommentsAndLocations
                ? new GraphQLVariableDefinition
                {
                    Variable = ParseVariable(),
                    Type = AdvanceThroughColonAndParseType(),
                    DefaultValue = SkipEqualsAndParseValueLiteral(),
                }
                : new GraphQLVariableDefinitionFull
                {
                    Comment = comment,
                    Variable = ParseVariable(),
                    Type = AdvanceThroughColonAndParseType(),
                    DefaultValue = SkipEqualsAndParseValueLiteral(),
                    Location = GetLocation(start)
                };
        }

        private List<GraphQLVariableDefinition>? ParseVariableDefinitions()
        {
            return Peek(TokenKind.PAREN_L) ?
                Many(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseVariableDefinition(), TokenKind.PAREN_R) :
                null;
        }

        private bool Peek(TokenKind kind) => _currentToken.Kind == kind;

        private bool Skip(TokenKind kind)
        {
            ParseComment();

            bool isCurrentTokenMatching = _currentToken.Kind == kind;

            if (isCurrentTokenMatching)
            {
                Advance();
            }

            return isCurrentTokenMatching;
        }

        private object? SkipEqualsAndParseValueLiteral() => Skip(TokenKind.EQUALS) ? ParseValueLiteral(true) : null;
    }
}
