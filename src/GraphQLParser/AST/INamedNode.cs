namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that has a name (type, argument, directive, field, operation, variable, etc.).
    /// </summary>
    public interface INamedNode
    {
        /// <summary>
        /// Name of the node represented as a nested node.
        /// </summary>
        GraphQLName? Name { get; set; }
    }
}
