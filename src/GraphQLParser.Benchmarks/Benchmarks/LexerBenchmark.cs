using BenchmarkDotNet.Attributes;

namespace GraphQLParser.Benchmarks
{
    [MemoryDiagnoser]
    public class LexerBenchmark : BenchmarkBase
    {
        [Benchmark]
        [ArgumentsSource(nameof(Names))]
        public void Lex(string name)
        {
            var lexer = new Lexer();
            var source = new Source(GetQueryByName(name));
            int resetPosition = 0;
            Token token;
            while ((token = lexer.Lex(source, resetPosition)).Kind != TokenKind.EOF)
            {
                resetPosition = token.End;
            }
        }

        public override void Run() => Lex("github");
    }
}
