using System;
using System.IO;
using System.Threading;
using BenchmarkDotNet.Running;

namespace GraphQLParser.Benchmarks
{
    internal static class Program
    {
        internal static string ReadGraphQLFile(this string name) => File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", name + ".graphql"));

        // Call without args for BenchmarkDotNet
        // Call with some arbitrary args for any memory profiler
        private static void Main(string[] args) => Run<LexerBenchmark>(args);

        private static void Run<TBenchmark>(string[] args)
            where TBenchmark : IBenchmark, new()
        {
            if (args.Length == 0)
                _ = BenchmarkRunner.Run<TBenchmark>();
            else
                RunMemoryProfilerPayload<TBenchmark>();
        }

        private static void RunMemoryProfilerPayload<TBenchmark>()
            where TBenchmark : IBenchmark, new()
        {
            var bench = new TBenchmark();
            bench.GlobalSetup();
            
            int count = 0;
            while (true)
            {
                bench.Run();

                Thread.Sleep(10);

                if (++count % 100 == 0)
                    Console.ReadLine();
            }
        }
    }
}
