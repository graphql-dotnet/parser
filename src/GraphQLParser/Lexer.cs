namespace GraphQLParser
{
    public class Lexer : ILexer
    {
        public ILexemeCache Cache { get; set; }

        public Token Lex(ISource source) => Lex(source, 0);

        public Token Lex(ISource source, int start)
        {
            var context = new LexerContext(source, start, Cache);
            return context.GetToken();
        }
    }
}