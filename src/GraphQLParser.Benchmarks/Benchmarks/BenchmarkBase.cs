using BenchmarkDotNet.Attributes;

namespace GraphQLParser.Benchmarks;

public abstract class BenchmarkBase : IBenchmark
{
    private string _hero = null!;
    private string _escapes = null!;
    private string _kitchen = null!;
    private string _introspection = null!;
    private string _params = null!;
    private string _variables = null!;
    private string _github = null!;

    [GlobalSetup]
    public virtual void GlobalSetup()
    {
        _hero = "hero".ReadGraphQLFile();
        _escapes = "query_with_many_escape_symbols".ReadGraphQLFile();
        _kitchen = "kitchenSink".ReadGraphQLFile();
        _introspection = "introspectionQuery".ReadGraphQLFile();
        _params = "params".ReadGraphQLFile();
        _variables = "variables".ReadGraphQLFile();
        _github = "github".ReadGraphQLFile();
    }

    public string GetQueryByName(string name)
    {
        return name switch
        {
            "hero" => _hero,
            "escapes" => _escapes,
            "kitchen" => _kitchen,
            "introspection" => _introspection,
            "params" => _params,
            "variables" => _variables,
            "github" => _github,
            _ => throw new System.Exception(name)
        };
    }

    public IEnumerable<string> Names()
    {
        yield return "hero";
        yield return "escapes";
        yield return "kitchen";
        yield return "introspection";
        yield return "params";
        yield return "variables";
        yield return "github";
    }

    public abstract void Run();
}
