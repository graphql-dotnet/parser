namespace GraphQLParser.AST
{
    /// <summary>
    /// Base AST node for <see cref="GraphQLOperationDefinition"/> and <see cref="GraphQLFragmentDefinition"/>.
    /// http://spec.graphql.org/October2021/#ExecutableDefinition
    /// </summary>
    public abstract class GraphQLExecutableDefinition : ASTNode, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public GraphQLDirectives? Directives { get; set; }

        /// <summary>
        /// Nested <see cref="GraphQLSelectionSet"/> AST node with selection set of this AST node.
        /// </summary>
        public GraphQLSelectionSet SelectionSet { get; set; } = null!;
    }
}
