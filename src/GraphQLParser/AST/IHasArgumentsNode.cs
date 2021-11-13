using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may have arguments.
    /// </summary>
    public interface IHasArgumentsNode
    {
        /// <summary>
        /// Arguments of the node represented as a list of nested nodes.
        /// </summary>
        List<GraphQLArgument>? Arguments { get; set; }
    }
}
