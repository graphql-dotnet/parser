using System.Diagnostics;

namespace GraphQLParser.AST
{
    [DebuggerDisplay("(Start={Start}, End={End})")]
    public readonly struct GraphQLLocation
    {
        public GraphQLLocation(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int End { get; }

        public int Start { get; }
    }
}