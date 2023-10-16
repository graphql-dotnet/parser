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

    /// <summary>
    /// Indicates whether this definition is a part of other definition. For example,
    /// <see cref="GraphQLEnumValueDefinition"/> is a part of <see cref="GraphQLEnumTypeDefinition"/>.
    /// </summary>
    public virtual bool IsChildDefinition => false;
}
