using System.IO;
using System.Threading.Tasks;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors;

public class SDLPrinterVerticalIndentationTests
{
    [Theory]
    [InlineData(1,
@"
extend schema @good
scalar A schema { query: Q }
",
@"extend schema @good

scalar A

schema {
  query: Q
}")]
    [InlineData(2,
@"scalar A1
scalar B
scalar C
scalar A2
",
@"scalar A1

scalar A2")]
    [InlineData(3,
@"scalar A1
scalar B
scalar A2
scalar E
scalar D
",
@"scalar A1

scalar A2")]
    [InlineData(4,
@"query A1 { x }
query B { y }
query A2 { z }
",
@"query A1 {
  x
}

query A2 {
  z
}")]
    [InlineData(5,
@"directive @A1 on FIELD
directive @B on FIELD
directive @A2 on FIELD
",
@"directive @A1 on FIELD

directive @A2 on FIELD")]
    [InlineData(6,
@"enum A1 { X Y }
enum B { X Y }
enum A2 { X Y }
enum E { X Y }
enum D { X Y }
",
@"enum A1 {
  X
  Y
}

enum A2 {
  X
  Y
}")]
    [InlineData(7,
@"extend enum A1 { X Y }
extend enum B { X Y }
extend enum A2 { X Y }
extend enum E { X Y }
extend enum D { X Y }
",
@"extend enum A1 {
  X
  Y
}

extend enum A2 {
  X
  Y
}")]
    [InlineData(8,
@"input A1 @vip
input B
input A2 { a: Int }
input E
input D
",
@"input A1 @vip

input A2 {
  a: Int
}")]
    [InlineData(9,
@"type A1 @vip
type B
type A2 { a: Int }
type E
type D
",
@"type A1 @vip

type A2 {
  a: Int
}")]
    [InlineData(10,
@"interface A1 @vip
interface B
interface A2 { a: Int }
interface E
interface D
",
@"interface A1 @vip

interface A2 {
  a: Int
}")]
    [InlineData(11,
@"extend interface A1 @vip
extend interface B { a: Int }
extend interface A2 { a: Int }
extend interface E { a: Int }
extend interface D { a: Int }
",
@"extend interface A1 @vip

extend interface A2 {
  a: Int
}")]
    [InlineData(12,
@"union A1 @vip
union B = X | Y
union A2 = X | Y
union E = X | Y
union D = X | Y
",
@"union A1 @vip

union A2 = X | Y")]
    [InlineData(13,
@"extend input A1 { a: Int }
extend input B { a: Int }
extend input A2 { a: Int }
extend input E { a: Int }
extend input D { a: Int }
",
@"extend input A1 {
  a: Int
}

extend input A2 {
  a: Int
}")]
    [InlineData(14,
@"extend type A1 { a: Int }
extend type B { a: Int }
extend type A2 { a: Int }
extend type E { a: Int }
extend type D { a: Int }
",
@"extend type A1 {
  a: Int
}

extend type A2 {
  a: Int
}")]
    public async Task Printer_Should_Print_Pretty_If_Definitions_Skipped(
int number,
string text,
string expected)
    {
        var printer = new MyPrinter();
        var writer = new StringWriter();
        var document = text.Parse();

        await printer.PrintAsync(document, writer).ConfigureAwait(false);
        var actual = writer.ToString();
        actual.ShouldBe(expected, $"Test {number} failed");

        actual.Parse(); // should be parsed back
    }

    private class MyPrinter : SDLPrinter
    {
        protected override ValueTask VisitObjectTypeExtensionAsync(GraphQLObjectTypeExtension objectTypeExtension, DefaultPrintContext context)
        {
            return objectTypeExtension.Name.Value.Span[0] == 'A'
                ? base.VisitObjectTypeExtensionAsync(objectTypeExtension, context)
                : default;
        }

        protected override ValueTask VisitInputObjectTypeExtensionAsync(GraphQLInputObjectTypeExtension inputObjectTypeExtension, DefaultPrintContext context)
        {
            return inputObjectTypeExtension.Name.Value.Span[0] == 'A'
                ? base.VisitInputObjectTypeExtensionAsync(inputObjectTypeExtension, context)
                : default;
        }

        protected override ValueTask VisitUnionTypeDefinitionAsync(GraphQLUnionTypeDefinition unionTypeDefinition, DefaultPrintContext context)
        {
            return unionTypeDefinition.Name.Value.Span[0] == 'A'
                ? base.VisitUnionTypeDefinitionAsync(unionTypeDefinition, context)
                : default;
        }

        protected override ValueTask VisitInterfaceTypeExtensionAsync(GraphQLInterfaceTypeExtension interfaceTypeExtension, DefaultPrintContext context)
        {
            return interfaceTypeExtension.Name.Value.Span[0] == 'A'
                ? base.VisitInterfaceTypeExtensionAsync(interfaceTypeExtension, context)
                : default;
        }

        protected override ValueTask VisitInterfaceTypeDefinitionAsync(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, DefaultPrintContext context)
        {
            return interfaceTypeDefinition.Name.Value.Span[0] == 'A'
                ? base.VisitInterfaceTypeDefinitionAsync(interfaceTypeDefinition, context)
                : default;
        }

        protected override ValueTask VisitObjectTypeDefinitionAsync(GraphQLObjectTypeDefinition objectTypeDefinition, DefaultPrintContext context)
        {
            return objectTypeDefinition.Name.Value.Span[0] == 'A'
                ? base.VisitObjectTypeDefinitionAsync(objectTypeDefinition, context)
                : default;
        }

        protected override ValueTask VisitInputObjectTypeDefinitionAsync(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, DefaultPrintContext context)
        {
            return inputObjectTypeDefinition.Name.Value.Span[0] == 'A'
                ? base.VisitInputObjectTypeDefinitionAsync(inputObjectTypeDefinition, context)
                : default;
        }

        protected override ValueTask VisitEnumTypeExtensionAsync(GraphQLEnumTypeExtension enumTypeExtension, DefaultPrintContext context)
        {
            return enumTypeExtension.Name.Value.Span[0] == 'A'
                ? base.VisitEnumTypeExtensionAsync(enumTypeExtension, context)
                : default;
        }

        protected override ValueTask VisitDirectiveDefinitionAsync(GraphQLDirectiveDefinition directiveDefinition, DefaultPrintContext context)
        {
            return directiveDefinition.Name.Value.Span[0] == 'A'
                ? base.VisitDirectiveDefinitionAsync(directiveDefinition, context)
                : default;
        }

        protected override ValueTask VisitOperationDefinitionAsync(GraphQLOperationDefinition operationDefinition, DefaultPrintContext context)
        {
            return operationDefinition.Name.Value.Span[0] == 'A'
                ? base.VisitOperationDefinitionAsync(operationDefinition, context)
                : default;
        }

        protected override ValueTask VisitScalarTypeDefinitionAsync(GraphQLScalarTypeDefinition scalarTypeDefinition, DefaultPrintContext context)
        {
            return scalarTypeDefinition.Name.Value.Span[0] == 'A'
                ? base.VisitScalarTypeDefinitionAsync(scalarTypeDefinition, context)
                : default;
        }

        protected override ValueTask VisitEnumTypeDefinitionAsync(GraphQLEnumTypeDefinition enumTypeDefinition, DefaultPrintContext context)
        {
            return enumTypeDefinition.Name.Value.Span[0] == 'A'
                ? base.VisitEnumTypeDefinitionAsync(enumTypeDefinition, context)
                : default;
        }
    }
}
