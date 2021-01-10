using BenchmarkDotNet.Attributes;
using GraphQLParser.Exceptions;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    public class ParserBinaryBenchmark : IBenchmark
    {
        private string _binaryTest = null!;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _binaryTest = "BinaryTest".ReadGraphQLFile();
        }

        [Benchmark]
        public void ParseBinaryFile()
        {
            try
            {
                Parser.Parse(_binaryTest).Dispose();
            }
            catch (GraphQLSyntaxErrorException)
            {
            }
        }

        void IBenchmark.Run() => ParseBinaryFile();
    }
}
