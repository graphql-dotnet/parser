using System.Collections.Generic;
using System.IO;
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
        public void WriteTreeVisitor_Should_Print_Tree(string text, string expected)
        {
            var context = new TestContext();

            using (var document = text.Parse())
            {
                _structWriter.Visit(document, context);
                var actual = context.Writer.ToString();
                actual.ShouldBe(expected);
            }
        }
    }
}
