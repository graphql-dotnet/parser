using GraphQLParser.Visitors;

namespace GraphQLParser.Tests.Visitors;

public class SDLSorterTests
{
    [Theory]
    [InlineData("KitchenSink")]
    [InlineData("ExecutableSortTests")]
    [InlineData("DefinitionSortTests")]
    public void SortsSchemaDefinition(string file)
    {
        var input = file.ReadGraphQLFile();
        var parsed = Parser.Parse(input);
        SDLSorter.Sort(parsed);
        var actual = new SDLPrinter(new SDLPrinterOptions { PrintComments = true }).Print(parsed);
        actual.ShouldMatchApproved(o =>
        {
            o.NoDiff();
            o.WithFileExtension($"{file}.graphql");
            o.WithStringCompareOptions(StringCompareShould.IgnoreLineEndings);
        });
    }
}
