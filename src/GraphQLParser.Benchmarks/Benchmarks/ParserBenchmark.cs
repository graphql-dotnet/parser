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
            source.Parse().Dispose();
        }

        public override void Run() => Parse("params");
    }
}
