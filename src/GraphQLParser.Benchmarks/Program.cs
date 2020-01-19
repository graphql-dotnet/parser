using BenchmarkDotNet.Running;
using System;
using System.Linq;
using System.Threading;

namespace GraphQLParser.Benchmarks
{
    internal static class Program
    {
        // Call without args for BenchmarkDotNet
        // Call with some arbitrary args for any memory profiler
        private static void Main(string[] args)
        {
            if (args.Length == 0)
                BenchmarkRunner.Run<ParserBenchmark>();
            else
                RunMemoryProfilerPayload();
        }

        private static void RunMemoryProfilerPayload()
        {
            var bench = new ParserBenchmark();
            bench.GlobalSetup();
            var queries = bench.Queries().ToArray();
            
            int count = 0;
            while (true)
            {
                bench.Concurrent(queries[2]);

                Thread.Sleep(10);

                ++count;
                if (count == 500)
                    break;
            }

            Console.WriteLine("end");
            Console.ReadLine();
        }
    }
}
