namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FragmentName"/>.
/// </summary>
public class GraphQLFragmentName : ASTNode, INamedNode
{
    internal GraphQLFragmentName()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLFragmentName"/>.
    /// </summary>
    public GraphQLFragmentName(GraphQLName name)
    {
        Name = name;

    }
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FragmentName;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }
}

internal sealed class GraphQLFragmentNameWithLocation : GraphQLFragmentName
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLFragmentNameWithComment : GraphQLFragmentName
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLFragmentNameFull : GraphQLFragmentName
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
