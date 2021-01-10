using System;
using GraphQLParser.AST;

namespace GraphQLParser
{
    public static class Parser
    {
        public static GraphQLDocument Parse(ReadOnlyMemory<char> source)
        {
            using var context = new ParserContext(source);
            return context.Parse();
        }
    }
}
