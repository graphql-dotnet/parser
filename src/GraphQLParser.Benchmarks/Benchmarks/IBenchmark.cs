namespace GraphQLParser.Benchmarks
{
    internal interface IBenchmark
    {
        void GlobalSetup();

        void Run();
    }
}
