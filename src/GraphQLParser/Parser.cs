using System;
using GraphQLParser.AST;

namespace GraphQLParser
{
    public struct Parser
    {
        private readonly Lexer _lexer;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
        }

        // only for tests
        internal GraphQLDocument Parse(string source) => Parse(source.AsMemory());

        public GraphQLDocument Parse(ReadOnlyMemory<char> source)
        {
            using var context = new ParserContext(source, _lexer);
            return context.Parse();
        }
    }
}
