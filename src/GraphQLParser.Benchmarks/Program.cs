using BenchmarkDotNet.Running;
using System;
using System.Threading;

namespace GraphQLParser.Benchmarks
{
    internal static class Program
    {
        //private static void Main()
        //{
        //    var bench = new ParserBenchmark();
        //    while (true)
        //    {
        //        bench.Parse();
        //    }
        //}

        private static void Main()
        {
            BenchmarkRunner.Run<ParserBenchmark>();
            Console.WriteLine("===DONE===");
            Console.ReadLine();
        }
    }
}
