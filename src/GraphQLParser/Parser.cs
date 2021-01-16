using System;
using GraphQLParser.AST;

namespace GraphQLParser
{
    public static class Parser
    {
        public static GraphQLDocument Parse(ReadOnlyMemory<char> source, bool ignoreComments = true) => new ParserContext(source, ignoreComments).Parse();
    }
}
