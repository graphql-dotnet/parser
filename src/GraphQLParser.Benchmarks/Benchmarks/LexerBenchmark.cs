using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    public class LexerBenchmark : IBenchmark
    {
        private readonly string _hero = "{ hero { id name } }";
        private string _escapes = null!;
        private string _kitchen = null!;
        private string _introspection = null!;
        private string _github = null!;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _escapes = "query_with_many_escape_symbols".ReadGraphQLFile();
            _kitchen = "kitchenSink".ReadGraphQLFile();
            _introspection = "introspectionQuery".ReadGraphQLFile();
            _github = "github".ReadGraphQLFile();
        }

        [Benchmark]
        [ArgumentsSource(nameof(Names))]
        public void Lex(string name)
        {
            var lexer = new Lexer();
            var source = GetQueryByName(name);
            int resetPosition = 0;
            Token token;
            while ((token = lexer.Lex(source, resetPosition)).Kind != TokenKind.EOF)
            {
                resetPosition = token.End;
            }
        }

        private string GetQueryByName(string name)
        {
            return name switch
            {
                "hero" => _hero,
                "escapes" => _escapes,
                "kitchen" => _kitchen,
                "introspection" => _introspection,
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
            yield return "github";
        }

        void IBenchmark.Run() => Lex("github");
    }
}
