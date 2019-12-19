using BenchmarkDotNet.Running;
using System.Linq;
using System.Threading;

namespace GraphQLParser.Benchmarks
{
    internal static class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<ParserBenchmark>();
        }

        private static void Main1()
        {
            var bench = new ParserBenchmark();
            bench.GlobalSetup();
            var queries = bench.Queries().ToArray();
            while (true)
            {
                bench.Parse(queries[2]);
                Thread.Sleep(10);
            }
        }
    }
}
