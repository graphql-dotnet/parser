using BenchmarkDotNet.Attributes;
using GraphQLParser.Exceptions;
using System.IO;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    public class ParserBinaryBenchmark
    {
        private static readonly string Binary = File.ReadAllText("BinaryTest.graphql");

        [Benchmark]
        public void ParseBinaryFile()
        {
            try
            {
                var parser = new Parser(new Lexer());
                parser.Parse(new Source(Binary));
            }
            catch (GraphQLSyntaxErrorException)
            {
            }
        }
    }
}
