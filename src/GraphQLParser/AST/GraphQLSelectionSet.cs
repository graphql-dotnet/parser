namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.SelectionSet"/>.
/// </summary>
public class GraphQLSelectionSet : ASTNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLSelectionSet"/>.
    /// </summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLSelectionSet()
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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLSelectionSetWithComment : GraphQLSelectionSet
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLSelectionSetFull : GraphQLSelectionSet
{
    private GraphQLLocation _location;
    private List<GraphQLComment>? _comments;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}
