using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors
{
    public class CountVisitorTests
    {
        [Theory]
        [InlineData("query a { name age }", 8)]
        [InlineData("scalar JSON @exportable", 6)]
        [InlineData("{a}", 5)] // Document->OperationDefinition->SelectionSet->Field->Name
        public async Task CountVisitor_Should_Count_Nodes(string text, int expectedCount)
        {
            var visitor = new CountVisitor<DefaultCountContext>();
            var context = new DefaultCountContext(_ => true);
            context.CancellationToken.ShouldBe(CancellationToken.None);

            using (var document = text.Parse())
            {
                await visitor.Visit(document, context).ConfigureAwait(false);
                context.Count.ShouldBe(expectedCount);
                document.AllNestedCount().ShouldBe(expectedCount);
            }
        }

        [Theory]
        [InlineData("query a { name age }")]
        [InlineData("scalar JSON @exportable")]
        [InlineData("{a}")]
        public async Task CountVisitor_Should_Count_Zero_Nodes(string text)
        {
            var visitor = new CountVisitor<DefaultCountContext>();
            var context = new DefaultCountContext(_ => false);
            context.CancellationToken.ShouldBe(CancellationToken.None);

            using (var document = text.Parse())
            {
                await visitor.Visit(document, context).ConfigureAwait(false);
                context.Count.ShouldBe(0);
            }
        }
    }
}
