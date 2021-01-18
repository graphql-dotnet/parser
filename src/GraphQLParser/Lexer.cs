namespace GraphQLParser
{
    public static class Lexer
    {
        public static Token Lex(ROM source, int start = 0) => new LexerContext(source, start).GetToken();
    }
}
