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

        /// <summary>
        /// The index for the start of the node in the source (i.e. it's inclusive).
        ///
        /// For example,
        ///     { field { subfield } }
        ///       ^ field.Location.Start = 2
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// The index for the character immediately after the node in the source (i.e. it's exclusive).
        ///
        /// For example,
        ///     { field { subfield } }
        ///                         ^ field.Location.End = 20
        /// </summary>
        public int End { get; }

        public bool Equals(GraphQLLocation other) => Start == other.Start && End == other.End;

        public override bool Equals(object obj) => obj is GraphQLLocation l && Equals(l);

        public override int GetHashCode() => (Start, End).GetHashCode();

        public override string ToString() => $"({Start},{End})";
    }
}
