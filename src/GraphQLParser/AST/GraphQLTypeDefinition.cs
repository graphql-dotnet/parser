namespace GraphQLParser.AST;

/// <summary>
/// Base AST node for all type definition nodes.
/// </summary>
public abstract class GraphQLTypeDefinition : ASTNode, INamedNode, IHasDescriptionNode
{
    /// <inheritdoc/>
    public GraphQLDescription? Description { get; set; }

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;
}
