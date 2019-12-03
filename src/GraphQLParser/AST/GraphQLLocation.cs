using System.Diagnostics;

namespace GraphQLParser.AST
{
    [DebuggerDisplay("(Start={Start}, End={End})")]
    public class GraphQLLocation
    {
        public int End { get; set; }

        public int Start { get; set; }
    }
}