namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Alias"/>.
/// </summary>
public class GraphQLAlias : ASTNode, INamedNode
{
    internal GraphQLAlias()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLAlias"/>.
    /// </summary>
    public GraphQLAlias(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Alias;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }
}

internal sealed class GraphQLAliasWithLocation : GraphQLAlias
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLAliasWithComment : GraphQLAlias
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLAliasFull : GraphQLAlias
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
