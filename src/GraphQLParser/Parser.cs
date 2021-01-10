using System;
using GraphQLParser.AST;

namespace GraphQLParser
{
    public static class Parser
    {
        // only for tests
        internal static GraphQLDocument Parse(string source) => Parse(source.AsMemory());

        public static GraphQLDocument Parse(ReadOnlyMemory<char> source)
        {
            using var context = new ParserContext(source);
            return context.Parse();
        }
    }
}
