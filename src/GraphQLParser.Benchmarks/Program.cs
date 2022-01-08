using System;
using System.IO;
using System.Threading;
using BenchmarkDotNet.Running;

namespace GraphQLParser.Benchmarks;

internal static class Program
{
    internal static string ReadGraphQLFile(this string name) => File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", name + ".graphql"));

    // Call without args for BenchmarkDotNet
    // Call with some arbitrary args for any profiler
    private static void Main(string[] args) => Run<ParserBenchmark>(args);

    private static void Run<TBenchmark>(string[] args)
        where TBenchmark : IBenchmark, new()
    {
        if (args.Length == 0)
            _ = BenchmarkRunner.Run<TBenchmark>();
        else
            RunProfilerPayload<TBenchmark>(100);
    }

    private static void RunProfilerPayload<TBenchmark>(int count)
        where TBenchmark : IBenchmark, new()
    {
        var bench = new TBenchmark();
        bench.GlobalSetup();

        int index = 0;
        while (true)
        {
            bench.Run();

            Thread.Sleep(10);

            if (++index % count == 0)
                Console.ReadLine();
        }
    }
}
