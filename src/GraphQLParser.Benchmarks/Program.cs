using BenchmarkDotNet.Running;
using System;

namespace GraphQLParser.Benchmarks
{
    internal static class Program
    {
        private static void Main()
        {
//            var bench = new LexerBenchmark();
//            bench.LexKitchenSink();
            BenchmarkRunner.Run<LexerBenchmark>();
            Console.WriteLine("===DONE===");
            Console.ReadLine();
        }
    }
}
