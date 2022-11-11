using GraphQLParser.Visitors;

namespace GraphQLParser.Tests.Visitors;

public class CountVisitorTests
{
    [Theory]
    [InlineData("query a { name age }", 8)]
    [InlineData("scalar JSON @exportable", 6)]
    [InlineData("{a}", 5)] // Document->OperationDefinition->SelectionSet->Field->Name
    [InlineData(""""
"""Very good type"""
type T {
# the best field ever
field: Int }
"""", 10)]
    [InlineData("mutation add($a: Int!, $b: [Int]) { result }", 19)]
    [InlineData("type T implements I & K { f: ID }", 13)]
    [InlineData("interface I implements K", 6)]
    [InlineData("query { field(list: [1, null, 2], obj: { x: true }, empty: { }) }", 21)]
    [InlineData("schema @exportable { query: Q mutation: M subscription: S }", 14)]
    [InlineData("union U = A | B", 8)]
    [InlineData("enum Color { RED GREEN }", 10)]
    [InlineData("input UserData { Login: String! Password: String! }", 14)]
    [InlineData("directive @skip(if: Boolean!) on FIELD | FRAGMENT_SPREAD | INLINE_FRAGMENT", 10)]
    [InlineData("extend schema @dir", 5)] // Document->SchemaExtension->Directives->Directive->Name
    [InlineData("extend scalar S @dir", 6)]
    [InlineData("extend type T @dir", 6)]
    [InlineData("extend interface I @dir", 6)]
    [InlineData("extend union U @dir", 6)]
    [InlineData("extend enum E @dir", 6)]
    [InlineData("extend input P @dir", 6)]
    public async Task CountVisitor_Should_Count_Nodes(string text, int expectedCount)
    {
        var visitor = new CountVisitor<DefaultCountContext>();
        var context = new DefaultCountContext(_ => true);
        context.CancellationToken.ShouldBe(CancellationToken.None);

        var document = text.Parse();

        await visitor.VisitAsync(document, context).ConfigureAwait(false);
        context.Count.ShouldBe(expectedCount);
        document.AllNestedCount().ShouldBe(expectedCount);
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

        var document = text.Parse();

        await visitor.VisitAsync(document, context).ConfigureAwait(false);
        context.Count.ShouldBe(0);
    }
}
