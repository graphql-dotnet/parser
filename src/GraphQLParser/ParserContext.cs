namespace GraphQLParser
{
    using AST;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ParserContext : IDisposable
    {
        private readonly ILexer lexer;
        private readonly ISource source;
        private readonly Stack<GraphQLComment> comments = new Stack<GraphQLComment>();
        private Token currentToken;

        public ParserContext(ISource source, ILexer lexer)
        {
            this.source = source;
            this.lexer = lexer;

            currentToken = this.lexer.Lex(source);
        }

        public void Dispose()
        {
            if (comments.Count > 0)
                throw new ApplicationException($"ParserContext has {comments.Count} not applied comments.");
        }

        public GraphQLComment GetComment() => comments.Count > 0 ? comments.Pop() : null;

        public GraphQLDocument Parse() => ParseDocument();

        private void Advance()
        {
            currentToken = lexer.Lex(source, currentToken.End);
        }

        private GraphQLType AdvanceThroughColonAndParseType()
        {
            Expect(TokenKind.COLON);
            return ParseType();
        }

        private IEnumerable<T> Any<T>(TokenKind open, Func<T> next, TokenKind close)
            where T : ASTNode
        {
            Expect(open);

            ParseComment();

            var nodes = new List<T>();
            while (!Skip(close))
                nodes.Add(next());

            return nodes;
        }

        private GraphQLDocument CreateDocument(int start, List<ASTNode> definitions)
        {
            return new GraphQLDocument
            {
                Location = new GraphQLLocation
                {
                    Start = start,
                    End = currentToken.End
                },
                Definitions = definitions
            };
        }

        private GraphQLFieldSelection CreateFieldSelection(int start, GraphQLName name, GraphQLName alias)
        {
            return new GraphQLFieldSelection
            {
                Alias = alias,
                Name = name,
                Arguments = ParseArguments(),
                Directives = ParseDirectives(),
                SelectionSet = Peek(TokenKind.BRACE_L) ? ParseSelectionSet() : null,
                Location = GetLocation(start)
            };
        }

        private ASTNode CreateGraphQLFragmentSpread(int start)
        {
            return new GraphQLFragmentSpread
            {
                Name = ParseFragmentName(),
                Directives = ParseDirectives(),
                Location = GetLocation(start)
            };
        }

        private ASTNode CreateInlineFragment(int start)
        {
            return new GraphQLInlineFragment
            {
                TypeCondition = GetTypeCondition(),
                Directives = ParseDirectives(),
                SelectionSet = ParseSelectionSet(),
                Location = GetLocation(start)
            };
        }

        private ASTNode CreateOperationDefinition(int start, OperationType operation, GraphQLName name)
        {
            var comment = GetComment();
            return new GraphQLOperationDefinition
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
            return new GraphQLOperationDefinition
            {
                Comment = comment,
                Operation = OperationType.Query,
                Directives = new GraphQLDirective[] { },
                SelectionSet = ParseSelectionSet(),
                Location = GetLocation(start)
            };
        }

        private void Expect(TokenKind kind)
        {
            if (currentToken.Kind == kind)
            {
                Advance();
            }
            else
            {
                throw new GraphQLSyntaxErrorException(
                    $"Expected {Token.GetTokenKindDescription(kind)}, found {currentToken}",
                    source,
                    currentToken.Start);
            }
        }

        private GraphQLValue ExpectColonAndParseValueLiteral(bool isConstant)
        {
            Expect(TokenKind.COLON);
            return ParseValueLiteral(isConstant);
        }

        private void ExpectKeyword(string keyword)
        {
            var token = currentToken;
            if (token.Kind == TokenKind.NAME && token.Value.Equals(keyword))
            {
                Advance();
                return;
            }

            throw new GraphQLSyntaxErrorException(
                    $"Expected \"{keyword}\", found Name \"{token.Value}\"", source, currentToken.Start);
        }

        private GraphQLNamedType ExpectOnKeywordAndParseNamedType()
        {
            ExpectKeyword("on");
            return ParseNamedType();
        }

        private GraphQLValue GetDefaultConstantValue()
        {
            GraphQLValue defaultValue = null;
            if (Skip(TokenKind.EQUALS))
            {
                defaultValue = ParseConstantValue();
            }

            return defaultValue;
        }

        private GraphQLLocation GetLocation(int start)
        {
            return new GraphQLLocation
            {
                Start = start,
                End = currentToken.End
            };
        }

        private GraphQLName GetName() => Peek(TokenKind.NAME) ? ParseName() : null;

        private GraphQLNamedType GetTypeCondition()
        {
            GraphQLNamedType typeCondition = null;
            if (currentToken.Value != null && currentToken.Value.Equals("on"))
            {
                Advance();
                typeCondition = ParseNamedType();
            }

            return typeCondition;
        }

        private IEnumerable<T> Many<T>(TokenKind open, Func<T> next, TokenKind close)
        {
            Expect(open);

            ParseComment();

            var nodes = new List<T> { next() };
            while (!Skip(close))
                nodes.Add(next());

            return nodes;
        }

        private GraphQLArgument ParseArgument()
        {
            var comment = GetComment();
            var start = currentToken.Start;

            return new GraphQLArgument
            {
                Comment = comment,
                Name = ParseName(),
                Value = ExpectColonAndParseValueLiteral(false),
                Location = GetLocation(start)
            };
        }

        private IEnumerable<GraphQLInputValueDefinition> ParseArgumentDefs()
        {
            if (!Peek(TokenKind.PAREN_L))
            {
                return new GraphQLInputValueDefinition[] { };
            }

            return Many(TokenKind.PAREN_L, () => ParseInputValueDef(), TokenKind.PAREN_R);
        }

        private IEnumerable<GraphQLArgument> ParseArguments()
        {
            return Peek(TokenKind.PAREN_L) ?
                Many(TokenKind.PAREN_L, () => ParseArgument(), TokenKind.PAREN_R) :
                new GraphQLArgument[] { };
        }

        private GraphQLValue ParseBooleanValue(Token token)
        {
            Advance();
            return new GraphQLScalarValue(ASTNodeKind.BooleanValue)
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
                ASTNode definition = null;
                if ((definition = ParseNamedDefinition()) != null)
                    return definition;
            }

            throw new GraphQLSyntaxErrorException(
                    $"Unexpected {currentToken}", source, currentToken.Start);
        }

        private IEnumerable<ASTNode> ParseDefinitionsIfNotEOF()
        {
            if (currentToken.Kind != TokenKind.EOF)
            {
                do
                {
                    yield return ParseDefinition();
                }
                while (!Skip(TokenKind.EOF));
            }
        }

        private GraphQLComment ParseComment()
        {
            if (!Peek(TokenKind.COMMENT))
            {
                return null;
            }

            var text = new List<string>();
            var start = currentToken.Start;
            int end;

            do
            {
                text.Add(currentToken.Value);
                end = currentToken.End;
                Advance();
            }
            while (currentToken.Kind == TokenKind.COMMENT);

            var comment = new GraphQLComment(string.Join(Environment.NewLine, text))
            {
                Location = new GraphQLLocation
                {
                    Start = start,
                    End = end
                }
            };

            comments.Push(comment);

            return comment;
        }

        private GraphQLDirective ParseDirective()
        {
            var start = currentToken.Start;
            Expect(TokenKind.AT);
            return new GraphQLDirective
            {
                Name = ParseName(),
                Arguments = ParseArguments(),
                Location = GetLocation(start)
            };
        }

        private GraphQLDirectiveDefinition ParseDirectiveDefinition()
        {
            var comment = GetComment();
            var start = currentToken.Start;
            ExpectKeyword("directive");
            Expect(TokenKind.AT);

            var name = ParseName();
            var args = ParseArgumentDefs();

            ExpectKeyword("on");
            var locations = ParseDirectiveLocations();

            return new GraphQLDirectiveDefinition
            {
                Comment = comment,
                Name = name,
                Arguments = args,
                Locations = locations,
                Location = GetLocation(start)
            };
        }

        private IEnumerable<GraphQLName> ParseDirectiveLocations()
        {
            var locations = new List<GraphQLName>();

            do
            {
                locations.Add(ParseName());
            }
            while (Skip(TokenKind.PIPE));

            return locations;
        }

        private IEnumerable<GraphQLDirective> ParseDirectives()
        {
            var directives = new List<GraphQLDirective>();
            while (Peek(TokenKind.AT))
                directives.Add(ParseDirective());

            return directives;
        }

        private GraphQLDocument ParseDocument()
        {
            int start = currentToken.Start;
            var definitions = ParseDefinitionsIfNotEOF().ToList();

            return CreateDocument(start, definitions);
        }

        private GraphQLEnumTypeDefinition ParseEnumTypeDefinition()
        {
            var comment = GetComment();
            var start = currentToken.Start;
            ExpectKeyword("enum");

            return new GraphQLEnumTypeDefinition
            {
                Comment = comment,
                Name = ParseName(),
                Directives = ParseDirectives(),
                Values = Many(TokenKind.BRACE_L, ParseEnumValueDefinition, TokenKind.BRACE_R),
                Location = GetLocation(start)
            };
        }

        private GraphQLValue ParseEnumValue(Token token)
        {
            Advance();
            return new GraphQLScalarValue(ASTNodeKind.EnumValue)
            {
                Value = token.Value,
                Location = GetLocation(token.Start)
            };
        }

        private GraphQLEnumValueDefinition ParseEnumValueDefinition()
        {
            var comment = GetComment();
            var start = currentToken.Start;

            return new GraphQLEnumValueDefinition
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
            var start = currentToken.Start;
            var name = ParseName();
            var args = ParseArgumentDefs();
            Expect(TokenKind.COLON);

            return new GraphQLFieldDefinition
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
            var start = currentToken.Start;
            var nameOrAlias = ParseName();
            GraphQLName name = null;
            GraphQLName alias = null;

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

            return CreateFieldSelection(start, name, alias);
        }

        private GraphQLValue ParseFloat(bool isConstant)
        {
            var token = currentToken;
            Advance();
            return new GraphQLScalarValue(ASTNodeKind.FloatValue)
            {
                Value = token.Value,
                Location = GetLocation(token.Start)
            };
        }

        private ASTNode ParseFragment()
        {
            var start = currentToken.Start;
            Expect(TokenKind.SPREAD);

            if (Peek(TokenKind.NAME) && !currentToken.Value.Equals("on"))
            {
                return CreateGraphQLFragmentSpread(start);
            }

            return CreateInlineFragment(start);
        }

        private GraphQLFragmentDefinition ParseFragmentDefinition()
        {
            var comment = GetComment();
            var start = currentToken.Start;
            ExpectKeyword("fragment");

            return new GraphQLFragmentDefinition
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
            if (currentToken.Value.Equals("on"))
            {
                throw new GraphQLSyntaxErrorException(
                    $"Unexpected {currentToken}", source, currentToken.Start);
            }

            return ParseName();
        }

        private IEnumerable<GraphQLNamedType> ParseImplementsInterfaces()
        {
            var types = new List<GraphQLNamedType>();
            if (currentToken.Value?.Equals("implements") == true)
            {
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
            var start = currentToken.Start;
            ExpectKeyword("input");

            return new GraphQLInputObjectTypeDefinition
            {
                Comment = comment,
                Name = ParseName(),
                Directives = ParseDirectives(),
                Fields = Any(TokenKind.BRACE_L, () => ParseInputValueDef(), TokenKind.BRACE_R),
                Location = GetLocation(start)
            };
        }

        private GraphQLInputValueDefinition ParseInputValueDef()
        {
            var comment = GetComment();
            var start = currentToken.Start;
            var name = ParseName();
            Expect(TokenKind.COLON);

            return new GraphQLInputValueDefinition
            {
                Comment = comment,
                Name = name,
                Type = ParseType(),
                DefaultValue = GetDefaultConstantValue(),
                Directives = ParseDirectives(),
                Location = GetLocation(start)
            };
        }

        private GraphQLValue ParseInt(bool isConstant)
        {
            var token = currentToken;
            Advance();

            return new GraphQLScalarValue(ASTNodeKind.IntValue)
            {
                Value = token.Value,
                Location = GetLocation(token.Start)
            };
        }

        private GraphQLInterfaceTypeDefinition ParseInterfaceTypeDefinition()
        {
            var comment = GetComment();
            var start = currentToken.Start;
            ExpectKeyword("interface");

            return new GraphQLInterfaceTypeDefinition
            {
                Comment = comment,
                Name = ParseName(),
                Directives = ParseDirectives(),
                Fields = Any(TokenKind.BRACE_L, () => ParseFieldDefinition(), TokenKind.BRACE_R),
                Location = GetLocation(start)
            };
        }

        private GraphQLValue ParseList(bool isConstant)
        {
            var start = currentToken.Start;
            Func<GraphQLValue> constant = () => ParseConstantValue();
            Func<GraphQLValue> value = () => ParseValueValue();

            return new GraphQLListValue(ASTNodeKind.ListValue)
            {
                Values = Any(TokenKind.BRACKET_L, isConstant ? constant : value, TokenKind.BRACKET_R),
                Location = GetLocation(start),
                AstValue = source.Body.Substring(start, currentToken.End - start - 1)
            };
        }

        private GraphQLName ParseName()
        {
            int start = currentToken.Start;
            var value = currentToken.Value;

            Expect(TokenKind.NAME);

            return new GraphQLName
            {
                Location = GetLocation(start),
                Value = value
            };
        }

        private ASTNode ParseNamedDefinition()
        {
            switch (currentToken.Value)
            {
                // Note: subscription is an experimental non-spec addition.
                case "query":
                case "mutation":
                case "subscription":
                    return ParseOperationDefinition();

                case "fragment": return ParseFragmentDefinition();

                // Note: the Type System IDL is an experimental non-spec addition.
                case "schema": return ParseSchemaDefinition();
                case "scalar": return ParseScalarTypeDefinition();
                case "type": return ParseObjectTypeDefinition();
                case "interface": return ParseInterfaceTypeDefinition();
                case "union": return ParseUnionTypeDefinition();
                case "enum": return ParseEnumTypeDefinition();
                case "input": return ParseInputObjectTypeDefinition();
                case "extend": return ParseTypeExtensionDefinition();
                case "directive": return ParseDirectiveDefinition();
                default: return null;
            }
        }

        private GraphQLNamedType ParseNamedType()
        {
            var start = currentToken.Start;
            return new GraphQLNamedType
            {
                Name = ParseName(),
                Location = GetLocation(start)
            };
        }

        private GraphQLValue ParseNameValue(bool isConstant)
        {
            var token = currentToken;

            if (token.Value.Equals("true") || token.Value.Equals("false"))
            {
                return ParseBooleanValue(token);
            }
            else if (token.Value != null)
            {
                if (token.Value.Equals("null"))
                    return ParseNullValue(token);
                else
                    return ParseEnumValue(token);
            }

            throw new GraphQLSyntaxErrorException(
                    $"Unexpected {currentToken}", source, currentToken.Start);
        }

        private GraphQLValue ParseObject(bool isConstant)
        {
            var comment = GetComment();
            var start = currentToken.Start;

            return new GraphQLObjectValue
            {
                Comment = comment,
                Fields = ParseObjectFields(isConstant),
                Location = GetLocation(start)
            };
        }

        private GraphQLValue ParseNullValue(Token token)
        {
            Advance();
            return new GraphQLScalarValue(ASTNodeKind.NullValue)
            {
                Value = null,
                Location = GetLocation(token.Start)
            };
        }

        private GraphQLObjectField ParseObjectField(bool isConstant)
        {
            var comment = GetComment();
            var start = currentToken.Start;
            return new GraphQLObjectField
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

            var start = currentToken.Start;
            ExpectKeyword("type");

            return new GraphQLObjectTypeDefinition
            {
                Comment = comment,
                Name = ParseName(),
                Interfaces = ParseImplementsInterfaces(),
                Directives = ParseDirectives(),
                Fields = Any(TokenKind.BRACE_L, () => ParseFieldDefinition(), TokenKind.BRACE_R),
                Location = GetLocation(start)
            };
        }

        private ASTNode ParseOperationDefinition()
        {
            var start = currentToken.Start;

            if (Peek(TokenKind.BRACE_L))
            {
                return CreateOperationDefinition(start);
            }

            return CreateOperationDefinition(start, ParseOperationType(), GetName());
        }

        private OperationType ParseOperationType()
        {
            var token = currentToken;
            Expect(TokenKind.NAME);

            switch (token.Value)
            {
                case "mutation": return OperationType.Mutation;
                case "subscription": return OperationType.Subscription;
                case "query":
                default: return OperationType.Query;
            }
        }

        private GraphQLOperationTypeDefinition ParseOperationTypeDefinition()
        {
            var start = currentToken.Start;
            var operation = ParseOperationType();
            Expect(TokenKind.COLON);
            var type = ParseNamedType();

            return new GraphQLOperationTypeDefinition
            {
                Operation = operation,
                Type = type,
                Location = GetLocation(start)
            };
        }

        private GraphQLScalarTypeDefinition ParseScalarTypeDefinition()
        {
            var comment = GetComment();
            var start = currentToken.Start;
            ExpectKeyword("scalar");
            var name = ParseName();
            var directives = ParseDirectives();

            return new GraphQLScalarTypeDefinition
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
            var start = currentToken.Start;
            ExpectKeyword("schema");
            var directives = ParseDirectives();
            var operationTypes = Many(TokenKind.BRACE_L, () => ParseOperationTypeDefinition(), TokenKind.BRACE_R);

            return new GraphQLSchemaDefinition
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
            var start = currentToken.Start;
            return new GraphQLSelectionSet
            {
                Selections = Many(TokenKind.BRACE_L, () => ParseSelection(), TokenKind.BRACE_R),
                Location = GetLocation(start)
            };
        }

        private GraphQLValue ParseString(bool isConstant)
        {
            var token = currentToken;
            Advance();
            return new GraphQLScalarValue(ASTNodeKind.StringValue)
            {
                Value = token.Value,
                Location = GetLocation(token.Start)
            };
        }

        private GraphQLType ParseType()
        {
            GraphQLType type = null;
            var start = currentToken.Start;
            if (Skip(TokenKind.BRACKET_L))
            {
                type = ParseType();
                Expect(TokenKind.BRACKET_R);
                type = new GraphQLListType
                {
                    Type = type,
                    Location = GetLocation(start)
                };
            }
            else
            {
                type = ParseNamedType();
            }

            if (Skip(TokenKind.BANG))
            {
                return new GraphQLNonNullType
                {
                    Type = type,
                    Location = GetLocation(start)
                };
            }

            return type;
        }

        private GraphQLTypeExtensionDefinition ParseTypeExtensionDefinition()
        {
            var comment = GetComment();
            var start = currentToken.Start;
            ExpectKeyword("extend");
            var definition = ParseObjectTypeDefinition();

            return new GraphQLTypeExtensionDefinition
            {
                Comment = comment,
                Definition = definition,
                Location = GetLocation(start)
            };
        }

        private IEnumerable<GraphQLNamedType> ParseUnionMembers()
        {
            var members = new List<GraphQLNamedType>();

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
            var start = currentToken.Start;
            ExpectKeyword("union");
            var name = ParseName();
            var directives = ParseDirectives();
            Expect(TokenKind.EQUALS);
            var types = ParseUnionMembers();

            return new GraphQLUnionTypeDefinition
            {
                Comment = comment,
                Name = name,
                Directives = directives,
                Types = types,
                Location = GetLocation(start)
            };
        }

        private GraphQLValue ParseValueLiteral(bool isConstant)
        {
            var token = currentToken;

            switch (token.Kind)
            {
                case TokenKind.BRACKET_L: return ParseList(isConstant);
                case TokenKind.BRACE_L: return ParseObject(isConstant);
                case TokenKind.INT: return ParseInt(isConstant);
                case TokenKind.FLOAT: return ParseFloat(isConstant);
                case TokenKind.STRING: return ParseString(isConstant);
                case TokenKind.NAME: return ParseNameValue(isConstant);
                case TokenKind.DOLLAR: if (!isConstant) return ParseVariable(); break;
            }

            throw new GraphQLSyntaxErrorException(
                    $"Unexpected {currentToken}", source, currentToken.Start);
        }

        private GraphQLValue ParseValueValue() => ParseValueLiteral(false);

        private GraphQLVariable ParseVariable()
        {
            var start = currentToken.Start;
            Expect(TokenKind.DOLLAR);

            return new GraphQLVariable
            {
                Name = GetName(),
                Location = GetLocation(start)
            };
        }

        private GraphQLVariableDefinition ParseVariableDefinition()
        {
            int start = currentToken.Start;
            return new GraphQLVariableDefinition
            {
                Variable = ParseVariable(),
                Type = AdvanceThroughColonAndParseType(),
                DefaultValue = SkipEqualsAndParseValueLiteral(),
                Location = GetLocation(start)
            };
        }

        private IEnumerable<GraphQLVariableDefinition> ParseVariableDefinitions()
        {
            return Peek(TokenKind.PAREN_L) ?
                Many(TokenKind.PAREN_L, () => ParseVariableDefinition(), TokenKind.PAREN_R) :
                new GraphQLVariableDefinition[] { };
        }

        private bool Peek(TokenKind kind) => currentToken.Kind == kind;

        private bool Skip(TokenKind kind)
        {
            ParseComment();

            var isCurrentTokenMatching = currentToken.Kind == kind;

            if (isCurrentTokenMatching)
            {
                Advance();
            }

            return isCurrentTokenMatching;
        }

        private object SkipEqualsAndParseValueLiteral()
        {
            return Skip(TokenKind.EQUALS) ? ParseValueLiteral(true) : null;
        }
    }
}