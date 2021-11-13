using System.Collections.Generic;
using GraphQLParser.AST;
using GraphQLParser.Exceptions;

namespace GraphQLParser
{
    // WARNING: mutable struct, pass it by reference to those methods that will change it
    internal partial struct ParserContext
    {
        // http://spec.graphql.org/October2021/#Document
        public GraphQLDocument ParseDocument()
        {
            int start = _currentToken.Start;
            var definitions = ParseDefinitionsIfNotEOF();

            SetCurrentComment(null); // push current (last) comment into _unattachedComments

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

        // http://spec.graphql.org/October2021/#TypeCondition
        private GraphQLTypeCondition? ParseTypeCondition(bool optional)
        {
            if (optional && _currentToken.Value != "on")
                return null;

            int start = _currentToken.Start;
            var condition = NodeHelper.CreateGraphQLTypeCondition(_ignoreOptions);
            condition.Comment = GetComment();
            ExpectKeyword("on");
            condition.Type = ParseNamedType();
            condition.Location = GetLocation(start);
            return condition;
        }

        // http://spec.graphql.org/October2021/#Argument
        private GraphQLArgument ParseArgument()
        {
            int start = _currentToken.Start;

            var arg = NodeHelper.CreateGraphQLArgument(_ignoreOptions);
            arg.Comment = GetComment();
            arg.Name = ParseName();

            Expect(TokenKind.COLON);

            arg.Value = ParseValueLiteral(false);
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

        // http://spec.graphql.org/October2021/#BooleanValue
        private GraphQLValue ParseBooleanValue()
        {
            var token = _currentToken;
            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.BooleanValue);
            val.Comment = GetComment();

            Advance();

            val.Value = token.Value;
            val.Location = GetLocation(token.Start);
            return val;
        }

        private ASTNode ParseDefinition()
        {
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

        // http://spec.graphql.org/October2021/#Comment
        private void ParseComment()
        {
            // skip comments
            if (_ignoreOptions.HasFlag(IgnoreOptions.Comments))
            {
                while (Peek(TokenKind.COMMENT))
                {
                    Advance(fromParseComment: true);
                }
                return;
            }

            if (!Peek(TokenKind.COMMENT))
            {
                return;
            }

            var text = new List<ROM>();
            int start = _currentToken.Start;
            int end;

            do
            {
                text.Add(_currentToken.Value);
                end = _currentToken.End;
                Advance(fromParseComment: true);
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
                (_document.RentedMemoryTracker ??= new List<(System.Buffers.IMemoryOwner<char>, ASTNode)>()).Add((owner, comment));
            }

            SetCurrentComment(comment);
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

        // http://spec.graphql.org/October2021/#Directive
        private GraphQLDirective ParseDirective()
        {
            int start = _currentToken.Start;
            var dir = NodeHelper.CreateGraphQLDirective(_ignoreOptions);
            dir.Comment = GetComment();

            Expect(TokenKind.AT);

            dir.Name = ParseName();
            dir.Arguments = ParseArguments();
            dir.Location = GetLocation(start);
            return dir;
        }

        /// <summary>
        /// http://spec.graphql.org/October2021/#DirectiveDefinition
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

        // http://spec.graphql.org/October2021/#Directives
        private List<GraphQLDirective>? ParseDirectives()
        {
            List<GraphQLDirective>? directives = null;
            while (Peek(TokenKind.AT))
                (directives ??= new List<GraphQLDirective>()).Add(ParseDirective());

            return directives;
        }

        // http://spec.graphql.org/October2021/#EnumTypeDefinition
        private GraphQLEnumTypeDefinition ParseEnumTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
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

        // http://spec.graphql.org/October2021/#EnumValue
        private GraphQLValue ParseEnumValue()
        {
            var token = _currentToken;
            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.EnumValue);
            val.Comment = GetComment();

            Advance();

            val.Value = token.Value;
            val.Location = GetLocation(token.Start);
            return val;
        }

        // http://spec.graphql.org/October2021/#EnumValueDefinition
        private GraphQLEnumValueDefinition ParseEnumValueDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
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

        // http://spec.graphql.org/October2021/#FieldDefinition
        private GraphQLFieldDefinition ParseFieldDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
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

        // http://spec.graphql.org/October2021/#Field
        private GraphQLField ParseField()
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

            var field = NodeHelper.CreateGraphQLField(_ignoreOptions);
            field.Alias = alias;
            field.Name = name;
            field.Arguments = ParseArguments();
            field.Directives = ParseDirectives();
            field.SelectionSet = Peek(TokenKind.BRACE_L) ? ParseSelectionSet() : null;
            field.Comment = comment;
            field.Location = GetLocation(start);
            return field;
        }

        // http://spec.graphql.org/October2021/#FloatValue
        private GraphQLValue ParseFloatValue(/*bool isConstant*/)
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
                ? ParseFragmentSpread(start, comment)
                : ParseInlineFragment(start, comment);
        }

        // http://spec.graphql.org/October2021/#FragmentSpread
        private GraphQLFragmentSpread ParseFragmentSpread(int start, GraphQLComment? comment)
        {
            var spread = NodeHelper.CreateGraphQLFragmentSpread(_ignoreOptions);
            spread.Name = ParseFragmentName();
            spread.Directives = ParseDirectives();
            spread.Comment = comment;
            spread.Location = GetLocation(start);
            return spread;
        }

        // http://spec.graphql.org/October2021/#InlineFragment
        private GraphQLInlineFragment ParseInlineFragment(int start, GraphQLComment? comment)
        {
            var frag = NodeHelper.CreateGraphQLInlineFragment(_ignoreOptions);
            frag.TypeCondition = ParseTypeCondition(optional: true);
            frag.Directives = ParseDirectives();
            frag.SelectionSet = ParseSelectionSet();
            frag.Comment = comment;
            frag.Location = GetLocation(start);
            return frag;
        }

        // http://spec.graphql.org/October2021/#FragmentDefinition
        private GraphQLFragmentDefinition ParseFragmentDefinition()
        {
            int start = _currentToken.Start;
            var comment = GetComment();
            ExpectKeyword("fragment");

            var def = NodeHelper.CreateGraphQLFragmentDefinition(_ignoreOptions);
            def.Name = ParseFragmentName();
            def.TypeCondition = ParseTypeCondition(optional: false);
            def.Directives = ParseDirectives();
            def.SelectionSet = ParseSelectionSet();
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        // http://spec.graphql.org/October2021/#FragmentName
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

        // http://spec.graphql.org/October2021/#ImplementsInterfaces
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

        // http://spec.graphql.org/October2021/#InputObjectTypeDefinition
        private GraphQLInputObjectTypeDefinition ParseInputObjectTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
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

        // http://spec.graphql.org/October2021/#InputValueDefinition
        private GraphQLInputValueDefinition ParseInputValueDef()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
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

        // http://spec.graphql.org/October2021/#IntValue
        private GraphQLValue ParseIntValue(/*bool isConstant*/)
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

        // http://spec.graphql.org/October2021/#InterfaceTypeDefinition
        private GraphQLInterfaceTypeDefinition ParseInterfaceTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
            }
            var comment = GetComment();

            ExpectKeyword("interface");

            var def = NodeHelper.CreateGraphQLInterfaceTypeDefinition(_ignoreOptions);
            def.Name = ParseName();
            def.Interfaces = ParseImplementsInterfaces();
            def.Directives = ParseDirectives();
            def.Fields = ParseFieldDefinitions();
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        // http://spec.graphql.org/October2021/#ListValue
        private GraphQLValue ParseListValue(bool isConstant)
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

        // http://spec.graphql.org/October2021/#Name
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

        // http://spec.graphql.org/October2021/#NamedType
        private GraphQLNamedType ParseNamedType()
        {
            int start = _currentToken.Start;
            var named = NodeHelper.CreateGraphQLNamedType(_ignoreOptions);
            named.Comment = GetComment();
            named.Name = ParseName();
            named.Location = GetLocation(start);
            return named;
        }

        private GraphQLValue ParseNameValue(/*bool isConstant*/)
        {
            var token = _currentToken;

            if (token.Value == "true" || token.Value == "false")
            {
                return ParseBooleanValue();
            }
            else if (!token.Value.IsEmpty)
            {
                return token.Value == "null"
                    ? ParseNullValue()
                    : ParseEnumValue();
            }

            return Throw_From_ParseNameValue();
        }

        private GraphQLValue Throw_From_ParseNameValue()
        {
            throw new GraphQLSyntaxErrorException($"Unexpected {_currentToken}", _source, _currentToken.Start);
        }

        // http://spec.graphql.org/October2021/#ObjectValue
        private GraphQLValue ParseObjectValue(bool isConstant)
        {
            int start = _currentToken.Start;
            var comment = GetComment();

            var val = NodeHelper.CreateGraphQLObjectValue(_ignoreOptions);
            val.Fields = ParseObjectFields(isConstant);
            val.Comment = comment;
            val.Location = GetLocation(start);
            return val;
        }

        // http://spec.graphql.org/October2021/#NullValue
        private GraphQLValue ParseNullValue()
        {
            var token = _currentToken;
            var val = NodeHelper.CreateGraphQLScalarValue(_ignoreOptions, ASTNodeKind.NullValue);
            val.Comment = GetComment();
            val.Value = token.Value;

            Advance();

            val.Location = GetLocation(token.Start);
            return val;
        }

        // http://spec.graphql.org/October2021/#ObjectField
        private GraphQLObjectField ParseObjectField(bool isConstant)
        {
            int start = _currentToken.Start;
            var comment = GetComment();
            var field = NodeHelper.CreateGraphQLObjectField(_ignoreOptions);
            field.Name = ParseName();

            Expect(TokenKind.COLON);

            field.Value = ParseValueLiteral(isConstant);
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

        // http://spec.graphql.org/October2021/#ObjectTypeDefinition
        private GraphQLObjectTypeDefinition ParseObjectTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
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

        // http://spec.graphql.org/October2021/#OperationDefinition
        private ASTNode ParseOperationDefinition()
        {
            int start = _currentToken.Start;
            var def = NodeHelper.CreateGraphQLOperationDefinition(_ignoreOptions);
            def.Comment = GetComment();

            if (Peek(TokenKind.BRACE_L))
            {
                def.Operation = OperationType.Query;
                def.SelectionSet = ParseSelectionSet();
                def.Location = GetLocation(start);
                return def;
            }
            else
            {
                def.Operation = ParseOperationType();
                def.Name = Peek(TokenKind.NAME) ? ParseName() : null; // Peek(TokenKind.NAME) because of anonymous query
                def.VariableDefinitions = ParseVariableDefinitions();
                def.Directives = ParseDirectives();
                def.SelectionSet = ParseSelectionSet();
                def.Location = GetLocation(start);
                return def;
            }
        }

        // http://spec.graphql.org/October2021/#OperationType
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

        // http://spec.graphql.org/October2021/#RootOperationTypeDefinition
        private GraphQLRootOperationTypeDefinition ParseRootOperationTypeDefinition()
        {
            int start = _currentToken.Start;
            var def = NodeHelper.CreateGraphQLOperationTypeDefinition(_ignoreOptions);
            def.Comment = GetComment();
            def.Operation = ParseOperationType();

            Expect(TokenKind.COLON);

            def.Type = ParseNamedType();
            def.Location = GetLocation(start);
            return def;
        }

        // http://spec.graphql.org/October2021/#ScalarTypeDefinition
        private GraphQLScalarTypeDefinition ParseScalarTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
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

        // http://spec.graphql.org/October2021/#SchemaDefinition
        private GraphQLSchemaDefinition ParseSchemaDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
            }
            var comment = GetComment();
            ExpectKeyword("schema");
            var directives = ParseDirectives();
            var operationTypes = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseRootOperationTypeDefinition(), TokenKind.BRACE_R);

            var def = NodeHelper.CreateGraphQLSchemaDefinition(_ignoreOptions);
            def.Directives = directives;
            def.OperationTypes = operationTypes;
            def.Description = description;
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        // http://spec.graphql.org/October2021/#Selection
        private ASTNode ParseSelection()
        {
            return Peek(TokenKind.SPREAD) ?
                ParseFragment() :
                ParseField();
        }

        // http://spec.graphql.org/October2021/#SelectionSet
        private GraphQLSelectionSet ParseSelectionSet()
        {
            int start = _currentToken.Start;
            var selection = NodeHelper.CreateGraphQLSelectionSet(_ignoreOptions);
            selection.Comment = GetComment();
            selection.Selections = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseSelection(), TokenKind.BRACE_R);
            selection.Location = GetLocation(start);
            return selection;
        }

        // http://spec.graphql.org/October2021/#StringValue
        private GraphQLScalarValue ParseStringValue(/*bool isConstant*/)
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

        // http://spec.graphql.org/October2021/#Description
        private GraphQLDescription ParseDescription()
        {
            var token = _currentToken;
            Advance();
            var descr = NodeHelper.CreateGraphQLDescription(_ignoreOptions);
            descr.Value = token.Value;
            descr.Location = GetLocation(token.Start);
            return descr;
        }

        // http://spec.graphql.org/October2021/#Type
        private GraphQLType ParseType()
        {
            GraphQLType type;
            int start = _currentToken.Start;
            if (Peek(TokenKind.BRACKET_L))
            {
                var listType = NodeHelper.CreateGraphQLListType(_ignoreOptions);
                listType.Comment = GetComment();

                Advance(); // skip BRACKET_L

                listType.Type = ParseType();

                Expect(TokenKind.BRACKET_R);

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
            // move comment from wrapped type to wrapping type
            nonNull.Comment = type.Comment;
            type.Comment = null;
            nonNull.Location = GetLocation(start);
            return nonNull;
        }

        // TODO: change name, spec does not containt term TypeExtensionDefinition
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

        // http://spec.graphql.org/October2021/#UnionMemberTypes
        private List<GraphQLNamedType> ParseUnionMemberTypes()
        {
            Expect(TokenKind.EQUALS);

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

        // http://spec.graphql.org/October2021/#UnionTypeDefinition
        private GraphQLUnionTypeDefinition ParseUnionTypeDefinition()
        {
            int start = _currentToken.Start;
            GraphQLDescription? description = null;
            if (Peek(TokenKind.STRING))
            {
                description = ParseDescription();
            }
            var comment = GetComment();
            ExpectKeyword("union");
            var name = ParseName();
            var directives = ParseDirectives();
            var types = ParseUnionMemberTypes();

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
            return _currentToken.Kind switch
            {
                TokenKind.BRACKET_L => ParseListValue(isConstant),
                TokenKind.BRACE_L => ParseObjectValue(isConstant),
                TokenKind.INT => ParseIntValue(/*isConstant*/),
                TokenKind.FLOAT => ParseFloatValue(/*isConstant*/),
                TokenKind.STRING => ParseStringValue(/*isConstant*/),
                TokenKind.NAME => ParseNameValue(/*isConstant*/),
                TokenKind.DOLLAR when !isConstant => ParseVariable(),
                _ => Throw_From_ParseValueLiteral()
            };
        }

        private GraphQLValue Throw_From_ParseValueLiteral()
        {
            throw new GraphQLSyntaxErrorException($"Unexpected {_currentToken}", _source, _currentToken.Start);
        }

        // http://spec.graphql.org/October2021/#Variable
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

        // http://spec.graphql.org/October2021/#VariableDefinition
        private GraphQLVariableDefinition ParseVariableDefinition()
        {
            int start = _currentToken.Start;
            var comment = GetComment();

            var def = NodeHelper.CreateGraphQLVariableDefinition(_ignoreOptions);
            def.Variable = ParseVariable();

            Expect(TokenKind.COLON);

            def.Type = ParseType();
            def.DefaultValue = Skip(TokenKind.EQUALS) ? ParseValueLiteral(true) : null;
            def.Directives = ParseDirectives();
            def.Comment = comment;
            def.Location = GetLocation(start);
            return def;
        }

        // http://spec.graphql.org/October2021/#VariableDefinitions
        private List<GraphQLVariableDefinition>? ParseVariableDefinitions()
        {
            return Peek(TokenKind.PAREN_L) ?
                OneOrMore(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseVariableDefinition(), TokenKind.PAREN_R) :
                null;
        }
    }
}
