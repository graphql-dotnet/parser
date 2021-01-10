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
            var source = GetQueryByName(name);
            Parser.Parse(source).Dispose();
        }

        public override void Run() => Parse("github");
    }
}
