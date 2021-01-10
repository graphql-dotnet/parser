using System;

namespace GraphQLParser
{
    public static class Lexer
    {
        // only for tests
        internal static Token Lex(string source, int start = 0) => Lex(source.AsMemory(), start);

        public static Token Lex(ReadOnlyMemory<char> source, int start = 0)
        {
            var context = new LexerContext(source, start);
            return context.GetToken();
        }
    }
}
