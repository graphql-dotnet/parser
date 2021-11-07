using System.Collections.Generic;
using GraphQLParser.AST;
using GraphQLParser.Exceptions;

namespace GraphQLParser
{
    // WARNING: mutable struct, pass it by reference to those methods that will change it
    internal partial struct ParserContext
    {
        public GraphQLDocument Parse() => ParseDocument();

        private GraphQLNamedType? ParseTypeCondition()
        {
            GraphQLNamedType? typeCondition = null;
            if (_currentToken.Value == "on")
            {
                Advance();
                typeCondition = ParseNamedType();
            }

            return typeCondition;
        }

        private GraphQLArgument ParseArgument()
        {
            int start = _currentToken.Start;
            var comment = GetComment();

            var arg = NodeHelper.CreateGraphQLArgument(_ignoreOptions);
            arg.Name = ParseName();
            arg.Value = ExpectColonAndParseValueLiteral(false);
            arg.Comment = comment;
            arg.Location = GetLocation(start);
            return arg;
        }

        private List<GraphQLInputValueDefinition>? ParseArgumentDefs()
        {
            return Peek(TokenKind.PAREN_L)
                ? OneOrMore(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseInputValueDef(), TokenKind.PAREN_R)
                : null;
        }

        private List<GraphQLInputValueDefinition>? ParseInputValueDefs()
        {
            return Peek(TokenKind.BRACE_L)
                ? OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseInputValueDef(), TokenKind.BRACE_R)
                : null;
        }

        private List<GraphQLArgument>? ParseArguments()
        {
            return Peek(TokenKind.PAREN_L)
                ? OneOrMore(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseArgument(), TokenKind.PAREN_R)
                : null;
        }

        private List<GraphQLFieldDefinition>? ParseFieldDefinitions()
        {
            return Peek(TokenKind.BRACE_L)
                ? OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseFieldDefinition(), TokenKind.BRACE_R)
                : null;
        }

        private List<GraphQLEnumValueDefinition>? ParseEnumValueDefinitions()
        {
            return Peek(TokenKind.BRACE_L)
                ? OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseEnumValueDefinition(), TokenKind.BRACE_R)
                : null;
        }

        private GraphQLValue ParseBooleanValue(Token token)
        {
            var comment = GetComment();
            Advance();

            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.BooleanValue);
            val.Value = token.Value;
            val.Comment = comment;
            val.Location = GetLocation(token.Start);
            return val;
        }

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

            if (Peek(TokenKind.STRING))
            {
                ASTNode? definition;
                if ((definition = ParseNamedDefinitionWithDescription()) != null)
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
            // skip comments
            if (_ignoreOptions.HasFlag(IgnoreOptions.Comments))
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

            var comment = NodeHelper.CreateGraphQLComment(_ignoreOptions);
            comment.Location = new GraphQLLocation(start, end);

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

        private GraphQLComment? GetComment()
        {
            var ret = _currentComment;
            _currentComment = null;
            return ret;
        }

        private GraphQLDirective ParseDirective()
        {
            int start = _currentToken.Start;
            Expect(TokenKind.AT);

            var dir = NodeHelper.CreateGraphQLDirective(_ignoreOptions);
            dir.Name = ParseName();
            dir.Arguments = ParseArguments();
            dir.Comment = null; //TODO: ????
            dir.Location = GetLocation(start);
            return dir;
        }

        /// <summary>
        /// http://spec.graphql.org/draft/#DirectiveDefinition
        /// DirectiveDefinition:
        ///     Description(opt) directive @ Name ArgumentsDefinition(opt) repeatable(opt) on DirectiveLocations
        /// </summary>
        /// <returns></returns>
        private GraphQLDirectiveDefinition ParseDirectiveDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();
            ExpectKeyword("directive");
            Expect(TokenKind.AT);

            var name = ParseName();
            var args = ParseArgumentDefs();
            bool repeatable = ParseRepeatable();

            ExpectKeyword("on");
            var locations = ParseDirectiveLocations();

            var def = NodeHelper.CreateGraphQLDirectiveDefinition(_ignoreOptions);
            def.Name = name;
            def.Repeatable = repeatable;
            def.Arguments = args;
            def.Locations = locations;
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
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
            _document = NodeHelper.CreateGraphQLDocument(_ignoreOptions);

            int start = _currentToken.Start;
            var definitions = ParseDefinitionsIfNotEOF();

            SetCurrentComment(null);

            _document.Location = new GraphQLLocation
            (
                start,
                // Formally, to denote the end of the document, it is better to use _prevToken.End,
                // since _prevToken represents some real meaningful token; _currentToken here is always EOF.
                // EOF is a technical token with length = 0, _prevToken.End and _currentToken.End have the same value here.
                _prevToken.End // equals to _currentToken.End (EOF)
            );
            _document.Definitions = definitions;
            _document.UnattachedComments = _unattachedComments;
            return _document;
        }

        private GraphQLEnumTypeDefinition ParseEnumTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();
            ExpectKeyword("enum");

            var def = NodeHelper.CreateGraphQLEnumTypeDefinition(_ignoreOptions);
            def.Name = ParseName();
            def.Directives = ParseDirectives();
            def.Values = ParseEnumValueDefinitions();
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLValue ParseEnumValue(Token token)
        {
            var comment = GetComment();
            Advance();

            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.EnumValue);
            val.Value = token.Value;
            val.Comment = comment;
            val.Location = GetLocation(token.Start);
            return val;
        }

        private GraphQLEnumValueDefinition ParseEnumValueDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();

            var def = NodeHelper.CreateGraphQLEnumValueDefinition(_ignoreOptions);
            def.Name = ParseName();
            def.Directives = ParseDirectives();
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLFieldDefinition ParseFieldDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }

            var comment = GetComment();
            var name = ParseName();
            var args = ParseArgumentDefs();
            Expect(TokenKind.COLON);

            var def = NodeHelper.CreateGraphQLFieldDefinition(_ignoreOptions);
            def.Name = name;
            def.Arguments = args;
            def.Type = ParseType();
            def.Directives = ParseDirectives();
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLFieldSelection ParseFieldSelection()
        {
            int start = _currentToken.Start;
            var comment = GetComment();
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

            var sel = NodeHelper.CreateGraphQLFieldSelection(_ignoreOptions);
            sel.Alias = alias;
            sel.Name = name;
            sel.Arguments = ParseArguments();
            sel.Directives = ParseDirectives();
            sel.SelectionSet = Peek(TokenKind.BRACE_L) ? ParseSelectionSet() : null;
            sel.Comment = comment;
            sel.Location = GetLocation(start);
            return sel;
        }

        private GraphQLValue ParseFloat(/*bool isConstant*/)
        {
            var token = _currentToken;
            var comment = GetComment();
            Advance();

            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.FloatValue);
            val.Value = token.Value;
            val.Comment = comment;
            val.Location = GetLocation(token.Start);
            return val;
        }

        private ASTNode ParseFragment()
        {
            int start = _currentToken.Start;
            var comment = GetComment();
            Expect(TokenKind.SPREAD);

            return Peek(TokenKind.NAME) && _currentToken.Value != "on"
                ? CreateGraphQLFragmentSpread(start, comment)
                : CreateInlineFragment(start, comment);
        }

        private ASTNode CreateGraphQLFragmentSpread(int start, GraphQLComment? comment)
        {
            var spread = NodeHelper.CreateGraphQLFragmentSpread(_ignoreOptions);
            spread.Name = ParseFragmentName();
            spread.Directives = ParseDirectives();
            spread.Comment = comment;
            spread.Location = GetLocation(start);
            return spread;
        }

        private ASTNode CreateInlineFragment(int start, GraphQLComment? comment)
        {
            var frag = NodeHelper.CreateGraphQLInlineFragment(_ignoreOptions);
            frag.TypeCondition = ParseTypeCondition();
            frag.Directives = ParseDirectives();
            frag.SelectionSet = ParseSelectionSet();
            frag.Comment = comment;
            frag.Location = GetLocation(start);
            return frag;
        }

        private GraphQLFragmentDefinition ParseFragmentDefinition()
        {
            int start = _currentToken.Start;
            var comment = GetComment();
            ExpectKeyword("fragment");

            var def = NodeHelper.CreateGraphQLFragmentDefinition(_ignoreOptions);
            def.Name = ParseFragmentName();
            def.TypeCondition = ExpectOnKeywordAndParseNamedType();
            def.Directives = ParseDirectives();
            def.SelectionSet = ParseSelectionSet();
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
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

                // Objects that implement interfaces may be defined with an optional leading & character
                // to aid formatting when representing a longer list of implemented interfaces
                Skip(TokenKind.AMPERSAND);

                do
                {
                    types.Add(ParseNamedType());
                }
                while (Skip(TokenKind.AMPERSAND));
            }

            return types;
        }

        private GraphQLInputObjectTypeDefinition ParseInputObjectTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();
            ExpectKeyword("input");

            var def = NodeHelper.CreateGraphQLInputObjectTypeDefinition(_ignoreOptions);
            def.Name = ParseName();
            def.Directives = ParseDirectives();
            def.Fields = ParseInputValueDefs();
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLInputValueDefinition ParseInputValueDef()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();
            var name = ParseName();
            Expect(TokenKind.COLON);

            var def = NodeHelper.CreateGraphQLInputValueDefinition(_ignoreOptions);
            def.Name = name;
            def.Type = ParseType();
            def.DefaultValue = Skip(TokenKind.EQUALS) ? ParseValueLiteral(true) : null;
            def.Directives = ParseDirectives();
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLValue ParseInt(/*bool isConstant*/)
        {
            var token = _currentToken;
            var comment = GetComment();
            Advance();

            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.IntValue);
            val.Value = token.Value;
            val.Comment = comment;
            val.Location = GetLocation(token.Start);
            return val;
        }

        private GraphQLInterfaceTypeDefinition ParseInterfaceTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();
            ExpectKeyword("interface");

            var def = NodeHelper.CreateGraphQLInterfaceTypeDefinition(_ignoreOptions);
            def.Name = ParseName();
            def.Directives = ParseDirectives();
            def.Fields = ParseFieldDefinitions();
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLValue ParseList(bool isConstant)
        {
            int start = _currentToken.Start;
            var comment = GetComment();

            // the compiler caches these delegates in the generated code
            ParseCallback<GraphQLValue> constant = (ref ParserContext context) => context.ParseValueLiteral(true);
            ParseCallback<GraphQLValue> value = (ref ParserContext context) => context.ParseValueLiteral(false);

            var val = NodeHelper.CreateGraphQLListValue(_ignoreOptions, ASTNodeKind.ListValue);
            val.Values = ZeroOrMore(TokenKind.BRACKET_L, isConstant ? constant : value, TokenKind.BRACKET_R);
            val.AstValue = _source.Slice(start, _currentToken.End - start - 1);
            val.Comment = comment;
            val.Location = GetLocation(start);
            return val;
        }

        private GraphQLName ParseName()
        {
            int start = _currentToken.Start;
            var value = _currentToken.Value;
            var comment = GetComment();

            Expect(TokenKind.NAME);

            var n = NodeHelper.CreateGraphQLName(_ignoreOptions);
            n.Value = value;
            n.Comment = comment;
            n.Location = GetLocation(start);
            return n;
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

        private ASTNode? ParseNamedDefinitionWithDescription()
        {
            // look-ahead to next token (_currentToken remains unchanged)
            var token = Lexer.Lex(_source, _currentToken.End);
            // skip comments
            while (token.Kind != TokenKind.EOF && token.Kind == TokenKind.COMMENT)
            {
                token = Lexer.Lex(_source, token.End);
            }
            // verify this is a NAME
            if (token.Kind != TokenKind.NAME)
                return null;

            // retrieve the value
            var value = token.Value;

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

            if (value == "directive")
                return ParseDirectiveDefinition();

            return null;
        }

        private GraphQLNamedType ParseNamedType()
        {
            int start = _currentToken.Start;
            var named = NodeHelper.CreateGraphQLNamedType(_ignoreOptions);
            named.Name = ParseName();
            named.Comment = null; //TODO: ????
            named.Location = GetLocation(start);
            return named;
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
            int start = _currentToken.Start;
            var comment = GetComment();

            var val = NodeHelper.CreateGraphQLObjectValue(_ignoreOptions);
            val.Fields = ParseObjectFields(isConstant);
            val.Comment = comment;
            val.Location = GetLocation(start);
            return val;
        }

        private GraphQLValue ParseNullValue(Token token)
        {
            var comment = GetComment();
            Advance();
            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.NullValue);
            val.Value = token.Value;
            val.Comment = comment;
            val.Location = GetLocation(token.Start);
            return val;
        }

        private GraphQLObjectField ParseObjectField(bool isConstant)
        {
            int start = _currentToken.Start;
            var comment = GetComment();
            var field = NodeHelper.CreateGraphQLObjectField(_ignoreOptions);
            field.Name = ParseName();
            field.Value = ExpectColonAndParseValueLiteral(isConstant);
            field.Comment = comment;
            field.Location = GetLocation(start);
            return field;
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
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();

            ExpectKeyword("type");

            var def = NodeHelper.CreateGraphQLObjectTypeDefinition(_ignoreOptions);
            def.Name = ParseName();
            def.Interfaces = ParseImplementsInterfaces();
            def.Directives = ParseDirectives();
            def.Fields = ParseFieldDefinitions();
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private ASTNode ParseOperationDefinition()
        {
            int start = _currentToken.Start;

            return Peek(TokenKind.BRACE_L)
                ? CreateOperationDefinition(start)
                : CreateOperationDefinition(start, ParseOperationType(), Peek(TokenKind.NAME) ? ParseName() : null); // Peek(TokenKind.NAME) because of anonymous query
        }

        private ASTNode CreateOperationDefinition(int start)
        {
            var comment = GetComment();
            var def = NodeHelper.CreateGraphQLOperationDefinition(_ignoreOptions);
            def.Operation = OperationType.Query;
            def.SelectionSet = ParseSelectionSet();
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private ASTNode CreateOperationDefinition(int start, OperationType operation, GraphQLName? name)
        {
            var comment = GetComment();
            var def = NodeHelper.CreateGraphQLOperationDefinition(_ignoreOptions);
            def.Operation = operation;
            def.Name = name;
            def.VariableDefinitions = ParseVariableDefinitions();
            def.Directives = ParseDirectives();
            def.SelectionSet = ParseSelectionSet();
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
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

            var def = NodeHelper.CreateGraphQLOperationTypeDefinition(_ignoreOptions);
            def.Operation = operation;
            def.Type = type;
            def.Comment = null; //TODO: ????
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLScalarTypeDefinition ParseScalarTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();
            ExpectKeyword("scalar");
            var name = ParseName();
            var directives = ParseDirectives();

            var def = NodeHelper.CreateGraphQLScalarTypeDefinition(_ignoreOptions);
            def.Name = name;
            def.Directives = directives;
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLSchemaDefinition ParseSchemaDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();
            ExpectKeyword("schema");
            var directives = ParseDirectives();
            var operationTypes = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseOperationTypeDefinition(), TokenKind.BRACE_R);

            var def = NodeHelper.CreateGraphQLSchemaDefinition(_ignoreOptions);
            def.Directives = directives;
            def.OperationTypes = operationTypes;
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
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
            var selection = NodeHelper.CreateGraphQLSelectionSet(_ignoreOptions);
            selection.Selections = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseSelection(), TokenKind.BRACE_R);
            selection.Comment = null; //TODO: ???
            selection.Location = GetLocation(start);
            return selection;
        }

        private GraphQLScalarValue ParseString(/*bool isConstant*/)
        {
            var token = _currentToken;
            var comment = GetComment();
            Advance();

            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.StringValue);
            val.Value = token.Value;
            val.Comment = comment;
            val.Location = GetLocation(token.Start);
            return val;
        }

        private GraphQLDescription ParseDescription()
        {
            var token = _currentToken;
            Advance();
            var descr = NodeHelper.CreateGraphQLDescription(_ignoreOptions);
            descr.Value = token.Value;
            descr.Location = GetLocation(token.Start);
            return descr;
        }

        private GraphQLType ParseType()
        {
            GraphQLType type;
            int start = _currentToken.Start;
            if (Skip(TokenKind.BRACKET_L))
            {
                type = ParseType();
                Expect(TokenKind.BRACKET_R);
                var listType = NodeHelper.CreateGraphQLListType(_ignoreOptions);
                listType.Type = type;
                listType.Comment = null; //TODO: ???????????????
                listType.Location = GetLocation(start);
                type = listType;
            }
            else
            {
                type = ParseNamedType();
            }

            if (!Skip(TokenKind.BANG))
                return type;

            var nonNull = NodeHelper.CreateGraphQLNonNullType(_ignoreOptions);
            nonNull.Type = type;
            nonNull.Comment = null; /////TODO: ????
            nonNull.Location = GetLocation(start);
            return nonNull;
        }

        private GraphQLTypeExtensionDefinition ParseTypeExtensionDefinition()
        {
            int start = _currentToken.Start;
            var comment = GetComment();
            ExpectKeyword("extend");
            var definition = ParseObjectTypeDefinition();

            // Note that due to the spec extension definitions have no descriptions.
            var def = NodeHelper.CreateGraphQLTypeExtensionDefinition(_ignoreOptions);
            def.Name = definition.Name;
            def.Definition = definition;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
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
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
                ParseComment();
            }
            var comment = GetComment();
            ExpectKeyword("union");
            var name = ParseName();
            var directives = ParseDirectives();
            Expect(TokenKind.EQUALS);
            var types = ParseUnionMembers();

            var def = NodeHelper.CreateGraphQLUnionTypeDefinition(_ignoreOptions);
            def.Name = name;
            def.Directives = directives;
            def.Types = types;
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private GraphQLValue ParseValueLiteral(bool isConstant)
        {
            ParseComment();

            return _currentToken.Kind switch
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
        }

        private GraphQLValue Throw_From_ParseValueLiteral()
        {
            throw new GraphQLSyntaxErrorException($"Unexpected {_currentToken}", _source, _currentToken.Start);
        }

        private GraphQLVariable ParseVariable()
        {
            int start = _currentToken.Start;
            var comment = GetComment();
            Expect(TokenKind.DOLLAR);

            var variable = NodeHelper.CreateGraphQLVariable(_ignoreOptions);
            variable.Name = ParseName();
            variable.Comment = comment;
            variable.Location = GetLocation(start);
            return variable;
        }

        private GraphQLVariableDefinition ParseVariableDefinition()
        {
            int start = _currentToken.Start;
            var comment = GetComment();

            var def = NodeHelper.CreateGraphQLVariableDefinition(_ignoreOptions);
            def.Variable = ParseVariable();
            def.Type = ExpectColonAndParseType();
            def.DefaultValue = Skip(TokenKind.EQUALS) ? ParseValueLiteral(true) : null;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        private List<GraphQLVariableDefinition>? ParseVariableDefinitions()
        {
            return Peek(TokenKind.PAREN_L) ?
                OneOrMore(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseVariableDefinition(), TokenKind.PAREN_R) :
                null;
        }
    }
}
