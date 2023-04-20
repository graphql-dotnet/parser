namespace GraphQLParser.AST;

/// <summary>
/// Base AST node for all type definition nodes.
/// </summary>
public abstract class GraphQLTypeDefinition : ASTNode, INamedNode, IHasDescriptionNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLTypeDefinition"/>.
    /// </summary>
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

    /// <summary>
    /// Indicates whether this definition is a part of other definition. For example,
    /// <see cref="GraphQLEnumValueDefinition"/> is a part of <see cref="GraphQLEnumTypeDefinition"/>.
    /// </summary>
    public virtual bool IsChildDefinition => false;
}
