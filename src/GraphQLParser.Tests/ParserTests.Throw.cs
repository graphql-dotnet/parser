using GraphQLParser.Exceptions;

namespace GraphQLParser.Tests;

public class ParserTestsThrow
{
    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void ParseNamedDefinitionWithDescription_At_EOF_Should_Throw(IgnoreOptions options)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "\"eof is near\"".Parse(new ParserOptions { Ignore = options }));
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:1) Unexpected String ""eof is near""
1: ""eof is near""
   ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Unexpected String \"eof is near\"");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(1);
    }

    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void Parse_Unicode_Char_At_EOF_Should_Throw(IgnoreOptions options)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "{\"\\ue }".Parse(new ParserOptions { Ignore = options }));
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:4) Invalid character escape sequence at EOF: \ue }.
1: {""\ue }
      ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Invalid character escape sequence at EOF: \\ue }.");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(4);
    }

    [Fact]
    public void Should_Throw_On_Unknown_OperationType()
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "superquery { a }".Parse());
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:1) Expected ""query/mutation/subscription/fragment/schema/scalar/type/interface/union/enum/input/extend/directive"", found Name ""superquery""
1: superquery { a }
   ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Expected \"query/mutation/subscription/fragment/schema/scalar/type/interface/union/enum/input/extend/directive\", found Name \"superquery\"");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(1);
    }

    [Theory]
    [InlineData("enum E { true A }", "Unexpected Name \"true\"; enum values are represented as unquoted names but not 'true' or 'false' or 'null'.", 1, 10)]
    [InlineData("enum E { B false }", "Unexpected Name \"false\"; enum values are represented as unquoted names but not 'true' or 'false' or 'null'.", 1, 12)]
    [InlineData("enum E { A null B }", "Unexpected Name \"null\"; enum values are represented as unquoted names but not 'true' or 'false' or 'null'.", 1, 12)]
    public void Should_Throw_On_Invalid_EnumValue(string query, string description, int line, int column)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => query.Parse());
        ex.Message.ShouldContain(description);
        ex.Description.ShouldBe(description);
        ex.Location.Line.ShouldBe(line);
        ex.Location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData("fragment u for User { name }", "Expected \"on\", found Name \"for\"", 1, 12)]
    [InlineData("fragment u 4 User { name }", "Expected \"on\", found Int \"4\"", 1, 12)]
    [InlineData("directive @d 4", "Expected \"on\", found Int \"4\"", 1, 14)]
    public void Should_Throw_On__Expect_Keyword(string query, string description, int line, int column)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => query.Parse());
        ex.Message.ShouldContain(description);
        ex.Description.ShouldBe(description);
        ex.Location.Line.ShouldBe(line);
        ex.Location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void Parse_Empty_Field_Arguments_Should_Throw(IgnoreOptions options)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "{ a() }".Parse(new ParserOptions { Ignore = options }));
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:5) Expected Name, found ); for more information see http://spec.graphql.org/October2021/#Argument
1: { a() }
       ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Expected Name, found ); for more information see http://spec.graphql.org/October2021/#Argument");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(5);
    }

    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void Parse_Empty_Directive_Arguments_Should_Throw(IgnoreOptions options)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "directive @dir() on FIELD_DEFINITION".Parse(new ParserOptions { Ignore = options }));
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:16) Expected Name, found ); for more information see http://spec.graphql.org/October2021/#InputValueDefinition
1: directive @dir() on FIELD_DEFINITION
                  ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Expected Name, found ); for more information see http://spec.graphql.org/October2021/#InputValueDefinition");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(16);
    }

    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void Parse_Empty_Enum_Values_Should_Throw(IgnoreOptions options)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "enum Empty { }".Parse(new ParserOptions { Ignore = options }));
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:14) Expected Name, found }; for more information see http://spec.graphql.org/October2021/#EnumValue
1: enum Empty { }
                ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Expected Name, found }; for more information see http://spec.graphql.org/October2021/#EnumValue");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(14);
    }

    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void Parse_Empty_SelectionSet_Should_Throw(IgnoreOptions options)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "{ a { } }".Parse(new ParserOptions { Ignore = options }));
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:7) Expected Name, found }; for more information see http://spec.graphql.org/October2021/#Field
1: { a { } }
         ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Expected Name, found }; for more information see http://spec.graphql.org/October2021/#Field");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(7);
    }

    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void Parse_Empty_VariableDefinitions_Should_Throw(IgnoreOptions options)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => "query test() { a }".Parse(new ParserOptions { Ignore = options }));
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:12) Expected $, found )
1: query test() { a }
              ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Expected $, found )");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(12);
    }

    [Theory]
    [InlineData("directive @dir repeatable on OOPS", 1, 30)]
    [InlineData("directive @dir(a: Int) repeatable on OOPS", 1, 38)]
    [InlineData("directive @dir on FIELD_DEFINITION | OOPS", 1, 38)]
    [InlineData("directive @dir on | OOPS | ENUM_VALUE", 1, 21)]
    [InlineData(@"directive @dir on
FIELD_DEFINITION | OOPS", 2, 20)]
    [InlineData(@"directive @dir on
OOPS
| ENUM_VALUE", 2, 1)]
    [InlineData(@"directive @dir on
| OOPS
| ENUM_VALUE", 2, 3)]
    [InlineData(@"directive @dir on
|  FIELD_DEFINITION
|          OOPS", 3, 12)]
    public void Should_Throw_On_Unknown_DirectiveLocation(string text, int line, int column)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse());
        ex.Message.ShouldContain(ex.Description);
        ex.Description.ShouldBe("Expected \"QUERY/MUTATION/SUBSCRIPTION/FIELD/FRAGMENT_DEFINITION/FRAGMENT_SPREAD/INLINE_FRAGMENT/VARIABLE_DEFINITION/SCHEMA/SCALAR/OBJECT/FIELD_DEFINITION/ARGUMENT_DEFINITION/INTERFACE/UNION/ENUM/ENUM_VALUE/INPUT_OBJECT/INPUT_FIELD_DEFINITION\", found Name \"OOPS\"");
        ex.Location.Line.ShouldBe(line);
        ex.Location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void Should_Fail_On_Empty_Fields(IgnoreOptions options)
    {
        string text = "interface Dog { }";
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse(new ParserOptions { Ignore = options }));
        ex.Message.ShouldBe(@"Syntax Error GraphQL (1:17) Expected Name, found }; for more information see http://spec.graphql.org/October2021/#FieldDefinition
1: interface Dog { }
                   ^
", StringCompareShould.IgnoreLineEndings);
        ex.Description.ShouldBe("Expected Name, found }; for more information see http://spec.graphql.org/October2021/#FieldDefinition");
        ex.Location.Line.ShouldBe(1);
        ex.Location.Column.ShouldBe(17);
    }

    [Theory]
    [InlineData(1, "directive @dir On FIELD_DEFINITION", "Unexpected Name \"On\"; did you miss 'repeatable'?", 1, 16)]
    [InlineData(2, "directive @dir onn FIELD_DEFINITION", "Unexpected Name \"onn\"; did you miss 'repeatable'?", 1, 16)]
    [InlineData(3, "directive @dir Repeatable on FIELD_DEFINITION", "Unexpected Name \"Repeatable\"; did you miss 'repeatable'?", 1, 16)]
    [InlineData(4, "directive @dir repeatablee on FIELD_DEFINITION", "Unexpected Name \"repeatablee\"; did you miss 'repeatable'?", 1, 16)]
    [InlineData(5, "directive @dir repeatable On FIELD_DEFINITION", "Expected \"on\", found Name \"On\"", 1, 27)]
    [InlineData(6, "directive @dir repeatable onn FIELD_DEFINITION", "Expected \"on\", found Name \"onn\"", 1, 27)]
    public void Should_Throw_GraphQLSyntaxErrorException(int number, string text, string description, int line, int column)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse());
        ex.Message.ShouldContain(ex.Description);
        ex.Description.ShouldBe(description, number.ToString());
        ex.Location.Line.ShouldBe(line);
        ex.Location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData("extend", "Expected \"schema/scalar/type/interface/union/enum/input\", found EOF", 1, 7)]
    [InlineData("extend variable", "Expected \"schema/scalar/type/interface/union/enum/input\", found Name \"variable\"", 1, 8)]
    [InlineData("extend schema", "Unexpected EOF; for more information see http://spec.graphql.org/October2021/#SchemaExtension", 1, 14)]
    [InlineData("extend schema A", "Unexpected Name \"A\"; for more information see http://spec.graphql.org/October2021/#SchemaExtension", 1, 15)]
    [InlineData("extend scalar", "Expected Name, found EOF; for more information see http://spec.graphql.org/October2021/#ScalarTypeExtension", 1, 14)]
    [InlineData("extend scalar A", "Unexpected EOF; for more information see http://spec.graphql.org/October2021/#ScalarTypeExtension", 1, 16)]
    [InlineData("extend scalar A B", "Unexpected Name \"B\"; for more information see http://spec.graphql.org/October2021/#ScalarTypeExtension", 1, 17)]
    [InlineData("extend type", "Expected Name, found EOF; for more information see http://spec.graphql.org/October2021/#ObjectTypeExtension", 1, 12)]
    [InlineData("extend type A", "Unexpected EOF; for more information see http://spec.graphql.org/October2021/#ObjectTypeExtension", 1, 14)]
    [InlineData("extend type A B", "Unexpected Name \"B\"; for more information see http://spec.graphql.org/October2021/#ObjectTypeExtension", 1, 15)]
    [InlineData("extend interface", "Expected Name, found EOF; for more information see http://spec.graphql.org/October2021/#InterfaceTypeExtension", 1, 17)]
    [InlineData("extend interface A", "Unexpected EOF; for more information see http://spec.graphql.org/October2021/#InterfaceTypeExtension", 1, 19)]
    [InlineData("extend interface A B", "Unexpected Name \"B\"; for more information see http://spec.graphql.org/October2021/#InterfaceTypeExtension", 1, 20)]
    [InlineData("extend union", "Expected Name, found EOF; for more information see http://spec.graphql.org/October2021/#UnionTypeExtension", 1, 13)]
    [InlineData("extend union A", "Unexpected EOF; for more information see http://spec.graphql.org/October2021/#UnionTypeExtension", 1, 15)]
    [InlineData("extend enum", "Expected Name, found EOF; for more information see http://spec.graphql.org/October2021/#EnumTypeExtension", 1, 12)]
    [InlineData("extend enum A", "Unexpected EOF; for more information see http://spec.graphql.org/October2021/#EnumTypeExtension", 1, 14)]
    [InlineData("extend input", "Expected Name, found EOF; for more information see http://spec.graphql.org/October2021/#InputObjectTypeExtension", 1, 13)]
    [InlineData("extend input A", "Unexpected EOF; for more information see http://spec.graphql.org/October2021/#InputObjectTypeExtension", 1, 15)]
    public void Should_Throw_Extensions(string text, string description, int line, int column)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse());
        ex.Message.ShouldContain(description);
        ex.Description.ShouldBe(description);
        ex.Location.Line.ShouldBe(line);
        ex.Location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData(1, "type Query { }", "Expected Name, found }; for more information see http://spec.graphql.org/October2021/#FieldDefinition", 1, 14)]
    [InlineData(2, "extend type Query { }", "Expected Name, found }; for more information see http://spec.graphql.org/October2021/#FieldDefinition", 1, 21)]
    [InlineData(3, "input Empty { }", "Expected Name, found }; for more information see http://spec.graphql.org/October2021/#InputValueDefinition", 1, 15)]
    [InlineData(4, "interface Empty { }", "Expected Name, found }; for more information see http://spec.graphql.org/October2021/#FieldDefinition", 1, 19)]
    [InlineData(5, "enum Empty { }", "Expected Name, found }; for more information see http://spec.graphql.org/October2021/#EnumValue", 1, 14)]
    [InlineData(6, "extend type Type implements Interface { }", "Expected Name, found }; for more information see http://spec.graphql.org/October2021/#FieldDefinition", 1, 41)]
    public void Should_Throw_On_Empty_Types_With_Braces(int number, string text, string description, int line, int column)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => text.Parse());
        ex.Message.ShouldContain(ex.Description);
        ex.Description.ShouldBe(description, number.ToString());
        ex.Location.Line.ShouldBe(line);
        ex.Location.Column.ShouldBe(column);
    }

    [Theory]
    [InlineData(@"
""misplaced description""
query { a }")]
    [InlineData(@"
""misplaced description""
mutation m { a }")]
    [InlineData(@"
""misplaced description""
subscription s { a }")]
    [InlineData(@"
""misplaced description""
fragment x on User { name }")]
    [InlineData(@"
""misplaced description""
extend type User implements Person")]
    public void Should_Throw_If_Descriptions_Not_Allowed(string query)
    {
        var ex = Should.Throw<GraphQLSyntaxErrorException>(() => query.Parse());
        ex.Message.ShouldContain(ex.Description);
        ex.Description.ShouldBe("Unexpected String \"misplaced description\"");
        ex.Location.Line.ShouldBe(2);
        ex.Location.Column.ShouldBe(1);
    }

    [Fact]
    public void Should_Throw_On_Unknown_Cases_From_ExpectOneOf()
    {
        var context = new ParserContext("abc", default);
        Should.Throw<NotSupportedException>(() => context.ParseNamedDefinition(new[] { "abc" }))
            .Message.ShouldBe("Unexpected keyword 'abc' in ParseNamedDefinition.");

        context = new ParserContext("abc", default);
        Should.Throw<NotSupportedException>(() => context.ParseOperationType(new[] { "abc" }))
            .Message.ShouldBe("Unexpected keyword 'abc' in ParseOperationType.");

        context = new ParserContext("abc", default);
        Should.Throw<NotSupportedException>(() => context.ParseDirectiveLocation(new[] { "abc" }))
            .Message.ShouldBe("Unexpected keyword 'abc' in ParseDirectiveLocation.");

        context = new ParserContext("extend abc", default);
        Should.Throw<NotSupportedException>(() => context.ParseTypeExtension(new[] { "abc" }))
            .Message.ShouldBe("Unexpected keyword 'abc' in ParseTypeExtension.");
    }
}
