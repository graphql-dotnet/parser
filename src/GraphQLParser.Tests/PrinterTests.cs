namespace GraphQLParser.Tests
{
    using GraphQLParser.AST;
    using Xunit;

    public class PrinterTests
    {
        private Printer printer;

        public PrinterTests()
        {
            this.printer = new Printer();
        }

        [Fact]
        public void Print_KitchenSink_DoesNotModifyAST()
        {
            var ast = this.Parse(ParserTests.LoadKitchenSink());
            var printed = this.printer.Print(ast);
            var ast2 = this.Parse(printed);

            AssertExtensions.DeepEqual(ast, ast2, typeof(GraphQLLocation));
        }

        private GraphQLDocument Parse(string body)
        {
            var parser = new Parser(new Lexer());

            return parser.Parse(new Source(body));
        }
    }
}
