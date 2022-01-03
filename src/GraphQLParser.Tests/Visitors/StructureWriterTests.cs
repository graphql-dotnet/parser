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

            public Stack<AST.ASTNode> Parents { get; set; } = new Stack<AST.ASTNode>();

            public CancellationToken CancellationToken { get; set; }
        }

        private static readonly StructureWriter<TestContext> _structWriter = new(new StructureWriterOptions());

        [Theory]
        [InlineData("query a { name age }", @"Document
  OperationDefinition
    Name [a]
    SelectionSet
      Field
        Name [name]
      Field
        Name [age]
")]
        [InlineData("scalar JSON @exportable", @"Document
  ScalarTypeDefinition
    Name [JSON]
    Directive
      Name [exportable]
")]
        [InlineData("{a}", @"Document
  OperationDefinition
    SelectionSet
      Field
        Name [a]
")]
        [InlineData(@"{
  a {
    b {
      c
    }
    d {
      e {
        f
      }
    }
    g {
      h {
        i {
          k
        }
      }
    }
  }
}", @"Document
  OperationDefinition
    SelectionSet
      Field
        Name [a]
        SelectionSet
          Field
            Name [b]
            SelectionSet
              Field
                Name [c]
          Field
            Name [d]
            SelectionSet
              Field
                Name [e]
                SelectionSet
                  Field
                    Name [f]
          Field
            Name [g]
            SelectionSet
              Field
                Name [h]
                SelectionSet
                  Field
                    Name [i]
                    SelectionSet
                      Field
                        Name [k]
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
