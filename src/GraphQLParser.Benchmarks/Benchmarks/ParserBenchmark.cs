using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace GraphQLParser.Benchmarks
{
    [Config(typeof(Config))]
    [MemoryDiagnoser]
    //[RPlotExporter, CsvMeasurementsExporter]
    public class ParserBenchmark : BenchmarkBase
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                SummaryStyle = new SummaryStyle(CultureInfo.InvariantCulture, printUnitsInHeader: true, sizeUnit: null, timeUnit: null, printUnitsInContent: false, printZeroValuesInContent: false, maxParameterColumnWidth: 40);
                Orderer = new ParserOrderer();
            }

            private class ParserOrderer : IOrderer
            {
                private static int GetOrder(object o) => o switch
                {
                    IgnoreOptions.None => 1,
                    IgnoreOptions.IgnoreComments => 2,
                    IgnoreOptions.IgnoreCommentsAndLocations => 3,
                    _ => 4
                };

                public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase) => benchmarksCase;

                public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCase, Summary summary) =>
                    from benchmark in benchmarksCase
                    orderby GetOrder(benchmark.Parameters["options"])
                    orderby benchmark.Parameters["name"]
                    select benchmark;

                public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) => null!;

                public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) => (string)benchmarkCase.Parameters["name"];

                public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups) => logicalGroups.OrderBy(it => it.Key);

                public bool SeparateLogicalGroups => true;
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(NamesAndOptions))]
        public void Parse(string name, IgnoreOptions options)
        {
            var source = GetQueryByName(name);
            Parser.Parse(source, new ParserOptions { Ignore = options }).Dispose();
        }

        public IEnumerable<object[]> NamesAndOptions()
        {
            yield return new object[] { "hero", IgnoreOptions.None };
            yield return new object[] { "hero", IgnoreOptions.IgnoreComments };
            yield return new object[] { "hero", IgnoreOptions.IgnoreCommentsAndLocations };

            yield return new object[] { "escapes", IgnoreOptions.None };
            yield return new object[] { "escapes", IgnoreOptions.IgnoreComments };
            yield return new object[] { "escapes", IgnoreOptions.IgnoreCommentsAndLocations };

            yield return new object[] { "kitchen", IgnoreOptions.None };
            yield return new object[] { "kitchen", IgnoreOptions.IgnoreComments };
            yield return new object[] { "kitchen", IgnoreOptions.IgnoreCommentsAndLocations };

            yield return new object[] { "introspection", IgnoreOptions.None };
            yield return new object[] { "introspection", IgnoreOptions.IgnoreComments };
            yield return new object[] { "introspection", IgnoreOptions.IgnoreCommentsAndLocations };

            yield return new object[] { "params", IgnoreOptions.None };
            yield return new object[] { "params", IgnoreOptions.IgnoreComments };
            yield return new object[] { "params", IgnoreOptions.IgnoreCommentsAndLocations };

            yield return new object[] { "variables", IgnoreOptions.None };
            yield return new object[] { "variables", IgnoreOptions.IgnoreComments };
            yield return new object[] { "variables", IgnoreOptions.IgnoreCommentsAndLocations };

            yield return new object[] { "github", IgnoreOptions.None };
            yield return new object[] { "github", IgnoreOptions.IgnoreComments };
            yield return new object[] { "github", IgnoreOptions.IgnoreCommentsAndLocations };
        }

        public override void Run() => Parse("params", IgnoreOptions.None);
    }
}
