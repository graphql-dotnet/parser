namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may have fields definition.
    /// </summary>
    public interface IHasFieldsDefinitionNode
    {
        /// <summary>
        /// Fields definition of the node represented as a nested node.
        /// </summary>
        GraphQLFieldsDefinition? Fields { get; set; }
    }
}
