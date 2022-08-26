using System.Collections.Immutable;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace GraphQLParser.Benchmarks;

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
                IgnoreOptions.Comments => 2,
                IgnoreOptions.Locations => 3,
                IgnoreOptions.All => 4,
                _ => 5
            };

            public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase, IEnumerable<BenchmarkLogicalGroupRule>? order = null) => benchmarksCase;

            public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCase, Summary summary) =>
                from benchmark in benchmarksCase
                orderby GetOrder(benchmark.Parameters["options"])
                orderby benchmark.Parameters["name"]
                select benchmark;

            public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) => null!;

            public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) => (string)benchmarkCase.Parameters["name"];

            public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups, IEnumerable<BenchmarkLogicalGroupRule>? order = null) => logicalGroups.OrderBy(it => it.Key);

            public bool SeparateLogicalGroups => true;
        }
    }

    [Benchmark]
    [ArgumentsSource(nameof(NamesAndOptions))]
    public void Parse(string name, IgnoreOptions options)
    {
        var source = GetQueryByName(name);
        Parser.Parse(source, new ParserOptions { Ignore = options });
    }

    public IEnumerable<object[]> NamesAndOptions()
    {
        yield return new object[] { "hero", IgnoreOptions.None };
        yield return new object[] { "hero", IgnoreOptions.Comments };
        yield return new object[] { "hero", IgnoreOptions.Locations };
        yield return new object[] { "hero", IgnoreOptions.All };

        yield return new object[] { "escapes", IgnoreOptions.None };
        yield return new object[] { "escapes", IgnoreOptions.Comments };
        yield return new object[] { "escapes", IgnoreOptions.Locations };
        yield return new object[] { "escapes", IgnoreOptions.All };

        yield return new object[] { "kitchen", IgnoreOptions.None };
        yield return new object[] { "kitchen", IgnoreOptions.Comments };
        yield return new object[] { "kitchen", IgnoreOptions.Locations };
        yield return new object[] { "kitchen", IgnoreOptions.All };

        yield return new object[] { "introspection", IgnoreOptions.None };
        yield return new object[] { "introspection", IgnoreOptions.Comments };
        yield return new object[] { "introspection", IgnoreOptions.Locations };
        yield return new object[] { "introspection", IgnoreOptions.All };

        yield return new object[] { "params", IgnoreOptions.None };
        yield return new object[] { "params", IgnoreOptions.Comments };
        yield return new object[] { "params", IgnoreOptions.Locations };
        yield return new object[] { "params", IgnoreOptions.All };

        yield return new object[] { "variables", IgnoreOptions.None };
        yield return new object[] { "variables", IgnoreOptions.Comments };
        yield return new object[] { "variables", IgnoreOptions.Locations };
        yield return new object[] { "variables", IgnoreOptions.All };

        yield return new object[] { "github", IgnoreOptions.None };
        yield return new object[] { "github", IgnoreOptions.Comments };
        yield return new object[] { "github", IgnoreOptions.Locations };
        yield return new object[] { "github", IgnoreOptions.All };
    }

    public override void Run() => Parse("params", IgnoreOptions.None);
}
