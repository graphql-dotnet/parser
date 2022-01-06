namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may implement interfaces.
    /// </summary>
    public interface IHasInterfacesNode
    {
        /// <summary>
        /// Nested <see cref="GraphQLImplementsInterfaces"/> AST node with interfaces implemented by this AST node.
        /// </summary>
        GraphQLImplementsInterfaces? Interfaces { get; set; }
    }
}
