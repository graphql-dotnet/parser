namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.SelectionSet"/>.
/// </summary>
public class GraphQLSelectionSet : ASTNode
{
    internal GraphQLSelectionSet()
    {
        Selections = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLSelectionSet"/>.
    /// </summary>
    public GraphQLSelectionSet(List<ASTNode> selections)
    {
        Selections = selections;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.SelectionSet;

    /// <summary>
    /// All selections in this set represented as a list of nested AST nodes.
    /// <br/>
    /// Available nodes:
    /// <list type="number">
    /// <item><see cref="GraphQLField"/></item>
    /// <item><see cref="GraphQLFragmentSpread"/></item>
    /// <item><see cref="GraphQLInlineFragment"/></item>
    /// </list>
    /// </summary>
    public List<ASTNode> Selections { get; set; }
}

internal sealed class GraphQLSelectionSetWithLocation : GraphQLSelectionSet
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLSelectionSetWithComment : GraphQLSelectionSet
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLSelectionSetFull : GraphQLSelectionSet
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
