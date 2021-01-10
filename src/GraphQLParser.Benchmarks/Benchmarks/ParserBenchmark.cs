using BenchmarkDotNet.Attributes;
using GraphQLParser.AST;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    //[RPlotExporter, CsvMeasurementsExporter]
    public class ParserBenchmark : BenchmarkBase
    {
        [Benchmark]
        [ArgumentsSource(nameof(Names))]
        public GraphQLDocument Parse(string name)
        {
            var parser = new Parser(new Lexer());
            var source = GetQueryByName(name);
            return parser.Parse(source);
        }

        public override void Run() => _ = Parse("github");
    }
}
