namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FragmentSpread"/>.
/// </summary>
public class GraphQLFragmentSpread : ASTNode, ISelectionNode, IHasDirectivesNode, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FragmentSpread;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLFragmentSpreadWithLocation : GraphQLFragmentSpread
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFragmentSpreadWithComment : GraphQLFragmentSpread
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLFragmentSpreadFull : GraphQLFragmentSpread
{
    private GraphQLLocation _location;
    private GraphQLComment? _comment;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}
