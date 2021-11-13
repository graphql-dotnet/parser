using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors
{
    public class StructureWriterTests
    {
        private class TestContext : IWriteContext
        {
            public TextWriter Writer { get; set; } = new StringWriter();

            public Stack<AST.ASTNode> Parent { get; set; } = new Stack<AST.ASTNode>();

            public CancellationToken CancellationToken { get; set; }
        }

        private static readonly StructureWriter<TestContext> _structWriter = new();

        [Theory]
        [InlineData("query a { name age }", @"Document
  OperationDefinition
    Name
    SelectionSet
      Field
        Name
      Field
        Name
")]
        [InlineData("scalar JSON @exportable", @"Document
  ScalarTypeDefinition
    Name
    Directive
      Name
")]
        public async Task WriteTreeVisitor_Should_Print_Tree(string text, string expected)
        {
            var context = new TestContext();

            using (var document = text.Parse())
            {
                await _structWriter.Visit(document, context).ConfigureAwait(false);
                var actual = context.Writer.ToString();
                actual.ShouldBe(expected);
            }
        }
    }
}
