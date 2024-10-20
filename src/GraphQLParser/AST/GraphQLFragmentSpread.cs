namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FragmentSpread"/>.
/// </summary>
public class GraphQLFragmentSpread : ASTNode, ISelectionNode, IHasDirectivesNode
{
    internal GraphQLFragmentSpread()
    {
        FragmentName = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLFragmentSpread"/>.
    /// </summary>
    public GraphQLFragmentSpread(GraphQLFragmentName name)
    {
        FragmentName = name;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FragmentSpread;

    /// <summary>
    /// Fragment name represented as a nested AST node.
    /// </summary>
    public GraphQLFragmentName FragmentName { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLFragmentSpreadWithLocation : GraphQLFragmentSpread
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLFragmentSpreadWithComment : GraphQLFragmentSpread
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLFragmentSpreadFull : GraphQLFragmentSpread
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
