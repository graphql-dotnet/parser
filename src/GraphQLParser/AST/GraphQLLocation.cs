using System;
using System.Diagnostics;

namespace GraphQLParser.AST
{
    [DebuggerDisplay("(Start={Start}, End={End})")]
    public readonly struct GraphQLLocation : IEquatable<GraphQLLocation>
    {
        public GraphQLLocation(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int End { get; }

        public int Start { get; }

        public bool Equals(GraphQLLocation other) => Start == other.Start && End == other.End;

        public override bool Equals(object obj) => obj is GraphQLLocation l ? Equals(l) : false;

        public override int GetHashCode() => (Start, End).GetHashCode();

        public override string ToString() => $"({Start},{End})";
    }
}
