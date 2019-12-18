using BenchmarkDotNet.Running;
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
            while (true)
            {
                bench.Parse();
                Thread.Sleep(10);
            }
        }
    }
}
