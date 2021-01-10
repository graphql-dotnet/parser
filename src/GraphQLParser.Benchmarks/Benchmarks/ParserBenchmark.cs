using BenchmarkDotNet.Attributes;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    //[RPlotExporter, CsvMeasurementsExporter]
    public class ParserBenchmark : BenchmarkBase
    {
        [Benchmark]
        [ArgumentsSource(nameof(Names))]
        public void Parse(string name)
        {
            var parser = new Parser(new Lexer());
            var source = GetQueryByName(name);
            parser.Parse(source).Dispose();
        }

        public override void Run() => Parse("github");
    }
}
