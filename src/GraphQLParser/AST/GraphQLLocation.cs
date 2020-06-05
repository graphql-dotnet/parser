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

        /// <summary>
        /// The index for the character immediately after the node in the source (i.e. it's exclusive).
        ///
        /// For example,
        ///     {field{subfield}}
        ///                     ^ field.Location.End = 16
        /// </summary>
        public int End { get; }

        /// <summary>
        /// The index for the start of the node in the source (i.e. it's inclusive).
        ///
        /// For example,
        ///     {field{subfield}}
        ///      ^ field.Location.Start = 1
        /// </summary>
        public int Start { get; }
    }
}
