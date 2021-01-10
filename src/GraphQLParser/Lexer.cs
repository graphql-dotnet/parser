using System;

namespace GraphQLParser
{
    public struct Lexer
    {
        public int? BufferSize { get; set; }

        // only for tests
        internal Token Lex(string source, int start = 0) => Lex(source.AsMemory(), start);

        public Token Lex(ReadOnlyMemory<char> source, int start = 0)
        {
            var context = new LexerContext(source, start, BufferSize);
            return context.GetToken();
        }
    }
}
