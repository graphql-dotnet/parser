using System.IO;
using BenchmarkDotNet.Attributes;
using GraphQLParser.Exceptions;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    public class ParserBinaryBenchmark
    {
        [Benchmark]
        public void ParseBinaryFile()
        {
            try
            {
                var parser = new Parser(new Lexer());
                parser.Parse(new Source(File.ReadAllText("BinaryTest.graphql")));
            }
            catch (GraphQLSyntaxErrorException)
            {
            }
        }
    }
}
