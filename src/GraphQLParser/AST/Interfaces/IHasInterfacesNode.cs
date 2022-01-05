namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may implement interfaces.
    /// </summary>
    public interface IHasInterfacesNode
    {
        /// <summary>
        /// Implemented interfaces of the node represented as a nested node.
        /// </summary>
        GraphQLImplementsInterfaces? Interfaces { get; set; }
    }
}
