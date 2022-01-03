namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may have arguments definition.
    /// </summary>
    public interface IHasArgumentsDefinitionNode
    {
        /// <summary>
        /// Arguments definition of the node represented as a nested node.
        /// </summary>
        GraphQLArgumentsDefinition? Arguments { get; set; }
    }
}
