using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may implement interfaces.
    /// </summary>
    public interface IHasInterfacesNode
    {
        /// <summary>
        /// Implemented interfaces of the node represented as a list of nested nodes.
        /// </summary>
        List<GraphQLNamedType>? Interfaces { get; set; }
    }
}
