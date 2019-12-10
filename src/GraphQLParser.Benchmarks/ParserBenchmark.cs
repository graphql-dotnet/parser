using BenchmarkDotNet.Attributes;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    public class ParserBenchmark
    {
        [Benchmark]
        public void Parse()
        {
            var parser = new Parser(new Lexer());
            parser.Parse(new Source(@"query test { field1 field2(id: 5) { name address } field3 }"));
        }
    }
}
