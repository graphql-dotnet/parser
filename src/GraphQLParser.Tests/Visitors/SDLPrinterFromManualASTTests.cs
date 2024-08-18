using GraphQLParser.Visitors;

namespace GraphQLParser.Tests.Visitors;

public class SDLPrinterFromManualASTTests
{
    [Fact]
    public async Task SelectionSet_Without_Parent_Should_Be_Printed_On_New_Line()
    {
        var selectionSet = new GraphQLSelectionSetWithComment { Selections = new List<ASTNode>() };
        var writer = new StringWriter();
        var printer = new SDLPrinter(new SDLPrinterOptions { PrintComments = true });
        await printer.PrintAsync(selectionSet, writer);
        writer.ToString().ShouldBe(@"{
}");
        selectionSet.Comments = new List<GraphQLComment> { new GraphQLComment("comment") };
        writer = new StringWriter();
        await printer.PrintAsync(selectionSet, writer);
        writer.ToString().ShouldBe(@"#comment
{
}");
    }

    [Fact]
    public async Task SelectionSet_Under_Operation_With_Null_Name_Should_Be_Printed_On_New_Line()
    {
        var def = new GraphQLOperationDefinition
        {
            SelectionSet = new GraphQLSelectionSetWithComment { Selections = new List<ASTNode>() }
        };
        var writer = new StringWriter();
        var printer = new SDLPrinter(new SDLPrinterOptions { PrintComments = true });
        await printer.PrintAsync(def, writer);
        writer.ToString().ShouldBe(@"{
}");
        def.SelectionSet.Comments = new List<GraphQLComment> { new GraphQLComment("comment") };
        writer = new StringWriter();
        await printer.PrintAsync(def, writer);
        writer.ToString().ShouldBe(@"#comment
{
}");
    }

    [Theory]
    [InlineData(
@"description",
@"""description""")]
    [InlineData(
@"description
multilined",
@"""""""
description
multilined
""""""")]
    public async Task Description_Without_Parent_Should_Be_Printed(string text, string expected)
    {
        var description = new GraphQLDescription(text);
        var writer = new StringWriter();
        var printer = new SDLPrinter();
        await printer.PrintAsync(description, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(
"a \u0000\u0001\u0002\u0003\u0004\u0005\u0006\u0007\u0008\u0009\u000A\u000B\u000C\u000D\u000E\u000F b",
@"""a \u0000\u0001\u0002\u0003\u0004\u0005\u0006\u0007\b\t\n\u000B\f\r\u000E\u000F b""")]
    [InlineData(
"a \u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001A\u001B\u001C\u001D\u001E\u001F b",
@"""a \u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001A\u001B\u001C\u001D\u001E\u001F b""")]
    [InlineData(                    // TODO: Change test condition?
"Test\r\nLine 2\rLine 3\nLine 4", """"
"""
Test
Line 2Line 3
Line 4
"""
"""")]
    public async Task Description_With_Escaped_Unicode_Should_Be_Printed(string text, string expected)
    {
        var description = new GraphQLDescription(text);
        var writer = new StringWriter();
        var printer = new SDLPrinter();
        await printer.PrintAsync(description, writer);
        writer.ToString().ShouldBe(expected);
    }

    [Fact]
    public async Task InputValueDefinition_Without_Parent_Should_Be_Printed()
    {
        var def = new GraphQLInputValueDefinition
        {
            Name = new GraphQLName("field"),
            Type = new GraphQLNamedType { Name = new GraphQLName("String") },
            DefaultValue = new GraphQLStringValue("abc")
        };

        var writer = new StringWriter();
        var printer = new SDLPrinter();
        await printer.PrintAsync(def, writer);
        writer.ToString().ShouldBe("field: String = \"abc\"");
    }

    [Fact]
    public async Task Directive_Without_Parent_Should_Be_Printed()
    {
        var directive = new GraphQLDirective { Name = new GraphQLName("upper") };
        var writer = new StringWriter();
        var printer = new SDLPrinter();
        await printer.PrintAsync(directive, writer);
        writer.ToString().ShouldBe("@upper");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Printer_Should_Print_DirectiveDefinition_Without_Locations(bool eachDirectiveLocationOnNewLine)
    {
        var printer = new SDLPrinter(new SDLPrinterOptions
        {
            EachDirectiveLocationOnNewLine = eachDirectiveLocationOnNewLine,
        });
        var document = new GraphQLDocument
        {
            Definitions = new List<ASTNode>
            {
                new GraphQLDirectiveDefinition
                {
                    Name = new GraphQLName("null_locations"),
                    Locations = new GraphQLDirectiveLocations() // Items is null
                },
                new GraphQLDirectiveDefinition
                {
                    Name = new GraphQLName("empty_locations"),
                    Locations = new GraphQLDirectiveLocations() { Items = new List<DirectiveLocation>() } // Items is empty
                },
                new GraphQLScalarTypeDefinition
                {
                    Name = new GraphQLName("AAA")
                }
            }
        };

        var actual = printer.Print(document);
        actual.ShouldBe("""
            directive @null_locations on

            directive @empty_locations on

            scalar AAA

            """);
    }

    [Fact]
    public void Printer_Should_Print_Enum_Without_Values()
    {
        var printer = new SDLPrinter();
        var document = new GraphQLDocument
        {
            Definitions = new List<ASTNode>
            {
                new GraphQLEnumTypeDefinition
                {
                    Name = new GraphQLName("EnumWithoutValuesDefinition"),
                    Values = null,
                },
                new GraphQLEnumTypeDefinition
                {
                    Name = new GraphQLName("EnumWithValuesDefinitionOfNullItems"),
                    Values = new GraphQLEnumValuesDefinition(),
                },
                new GraphQLEnumTypeDefinition
                {
                    Name = new GraphQLName("EnumWithValuesDefinitionOfEmptyItems"),
                    Values = new GraphQLEnumValuesDefinition { Items = new List<GraphQLEnumValueDefinition>() },
                },
            }
        };

        var actual = printer.Print(document);
        actual.ShouldBe("""
            enum EnumWithoutValuesDefinition

            enum EnumWithValuesDefinitionOfNullItems {
            }

            enum EnumWithValuesDefinitionOfEmptyItems {
            }

            """);
    }

    [Fact]
    public void Printer_Should_Print_Input_Without_Fields()
    {
        var printer = new SDLPrinter();
        var document = new GraphQLDocument
        {
            Definitions = new List<ASTNode>
            {
                new GraphQLInputObjectTypeDefinition
                {
                    Name = new GraphQLName("InputWithoutFieldsDefinition"),
                    Fields = null,
                },
                new GraphQLInputObjectTypeDefinition
                {
                    Name = new GraphQLName("InputWithFieldsDefinitionOfNullItems"),
                    Fields = new GraphQLInputFieldsDefinition(),
                },
                new GraphQLInputObjectTypeDefinition
                {
                    Name = new GraphQLName("InputWithFieldsDefinitionOfEmptyItems"),
                    Fields = new GraphQLInputFieldsDefinition { Items = new List<GraphQLInputValueDefinition>() },
                },
            }
        };

        var actual = printer.Print(document);
        actual.ShouldBe("""
            input InputWithoutFieldsDefinition

            input InputWithFieldsDefinitionOfNullItems {
            }

            input InputWithFieldsDefinitionOfEmptyItems {
            }

            """);
    }

    [Fact]
    public void Printer_Should_Print_Type_Without_Fields()
    {
        var printer = new SDLPrinter();
        var document = new GraphQLDocument
        {
            Definitions = new List<ASTNode>
            {
                new GraphQLObjectTypeDefinition
                {
                    Name = new GraphQLName("TypeWithoutFieldsDefinition"),
                    Fields = null,
                },
                new GraphQLObjectTypeDefinition
                {
                    Name = new GraphQLName("TypeWithFieldsDefinitionOfNullItems"),
                    Fields = new GraphQLFieldsDefinition(),
                },
                new GraphQLObjectTypeDefinition
                {
                    Name = new GraphQLName("TypeWithFieldsDefinitionOfEmptyItems"),
                    Fields = new GraphQLFieldsDefinition { Items = new List<GraphQLFieldDefinition>() },
                },
            }
        };

        var actual = printer.Print(document);
        actual.ShouldBe("""
            type TypeWithoutFieldsDefinition

            type TypeWithFieldsDefinitionOfNullItems {
            }

            type TypeWithFieldsDefinitionOfEmptyItems {
            }

            """);
    }
}
