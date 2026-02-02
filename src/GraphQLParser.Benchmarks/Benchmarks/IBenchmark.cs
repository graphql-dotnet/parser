namespace GraphQLParser.Benchmarks;

internal interface IBenchmark
{
    public void GlobalSetup();

    public void Run();
}
