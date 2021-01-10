using System;

namespace GraphQLParser
{
    public static class Lexer
    {
        public static Token Lex(ReadOnlyMemory<char> source, int start = 0) => new LexerContext(source, start).GetToken();
    }
}
