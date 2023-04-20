namespace GraphQLParser.AST;

/// <summary>
/// Base AST node for <see cref="GraphQLOperationDefinition"/> and <see cref="GraphQLFragmentDefinition"/>.
/// <br/>
/// http://spec.graphql.org/October2021/#ExecutableDefinition
/// </summary>
public abstract class GraphQLExecutableDefinition : ASTNode, IHasSelectionSetNode, IHasDirectivesNode
{
    internal GraphQLExecutableDefinition()
    {
        SelectionSet = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLExecutableDefinition"/>.
    /// </summary>
    protected GraphQLExecutableDefinition(GraphQLSelectionSet selectionSet)
    {
        SelectionSet = selectionSet;
    }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
#pragma warning disable CS8767 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public GraphQLSelectionSet SelectionSet { get; set; }
#pragma warning restore CS8767
}
