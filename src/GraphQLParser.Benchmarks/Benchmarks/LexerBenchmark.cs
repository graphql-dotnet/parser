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
            var source = GetQueryByName(name);
            int resetPosition = 0;
            Token token;
            while ((token = Lexer.Lex(source, resetPosition)).Kind != TokenKind.EOF)
            {
                resetPosition = token.End;
            }
        }

        public override void Run() => Lex("github");
    }
}
