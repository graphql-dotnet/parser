namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may have fields definition.
    /// </summary>
    public interface IHasFieldsDefinitionNode
    {
        /// <summary>
        /// Nested <see cref="GraphQLFieldsDefinition"/> AST node with fields definition of this AST node.
        /// </summary>
        GraphQLFieldsDefinition? Fields { get; set; }
    }
}
