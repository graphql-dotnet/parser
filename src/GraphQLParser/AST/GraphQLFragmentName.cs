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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFragmentNameWithComment : GraphQLFragmentName
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLFragmentNameFull : GraphQLFragmentName
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
