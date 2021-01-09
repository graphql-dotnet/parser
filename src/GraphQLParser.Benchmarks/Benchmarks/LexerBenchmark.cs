using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    public class LexerBenchmark : IBenchmark
    {
        private string _escapes = null!;
        private string _kitchen = null!;
        private string _github = null!;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _escapes = "query_with_many_escape_symbols".ReadGraphQLFile();
            _kitchen = "kitchenSink".ReadGraphQLFile();
            _github = "github".ReadGraphQLFile();
        }

        [Benchmark]
        [ArgumentsSource(nameof(Queries))]
        public void Lex(string query)
        {
            var lexer = new Lexer();
            var source = new Source(query);
            int resetPosition = 0;
            Token token;
            while ((token = lexer.Lex(source, resetPosition)).Kind != TokenKind.EOF)
            {
                resetPosition = token.End;
            }
        }

        public IEnumerable<string> Queries()
        {
            yield return _escapes;
            yield return _kitchen;
            yield return _github;
        }

        void IBenchmark.Run() => Lex(_github);
    }
}
