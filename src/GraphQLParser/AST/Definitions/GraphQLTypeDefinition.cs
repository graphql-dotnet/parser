namespace GraphQLParser.AST;

/// <summary>
/// Base AST node for all type definition nodes.
/// </summary>
public abstract class GraphQLTypeDefinition : ASTNode, INamedNode, IHasDescriptionNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLTypeDefinition"/>.
    /// </summary>
    [Obsolete("This constructor will be removed in v9.")]
    protected GraphQLTypeDefinition()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLTypeDefinition"/>.
    /// </summary>
    protected GraphQLTypeDefinition(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public GraphQLDescription? Description { get; set; }

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }
}
