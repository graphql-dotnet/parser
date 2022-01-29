using System.IO;
using System.Threading.Tasks;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors;

public class SDLPrinterSkipDirectivesTests
{
    [Theory]
    [InlineData(1,
@"type Foo {
  f: Int @aliased
  k: Boolean
}
",
@"type Foo {
  f: Int @aliased
  k: Boolean
}")]
    [InlineData(2,
@"type Foo {
  f: Int @bad @aliased
  k: Boolean
}
",
@"type Foo {
  f: Int @aliased
  k: Boolean
}")]
    [InlineData(3,
@"type Foo {
  f: Int @aliased @bad
  k: Boolean
}
",
@"type Foo {
  f: Int @aliased
  k: Boolean
}")]
    [InlineData(4,
@"type Foo {
  f: Int @bad
  k: Boolean
}
",
@"type Foo {
  f: Int
  k: Boolean
}")]
    public async Task Printer_Should_Print_Pretty_If_Direcives_Skipped(
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
        protected override ValueTask VisitDirectiveAsync(GraphQLDirective directive, DefaultPrintContext context)
        {
            return directive.Name.Value.Span[0] == 'a'
               ? base.VisitDirectiveAsync(directive, context)
               : default;
        }
    }
}
