namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Field"/>.
/// </summary>
public class GraphQLField : ASTNode, ISelectionNode, IHasSelectionSetNode, IHasDirectivesNode, IHasArgumentsNode, INamedNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLField"/>.
    /// </summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLField()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLField"/>.
    /// </summary>
    public GraphQLField(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Field;

    /// <summary>
    /// Nested <see cref="GraphQLAlias"/> AST node with field alias (if any).
    /// </summary>
    public GraphQLAlias? Alias { get; set; }

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }

    /// <summary>
    /// Arguments for this field.
    /// </summary>
    public GraphQLArguments? Arguments { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
    public GraphQLSelectionSet? SelectionSet { get; set; }
}

internal sealed class GraphQLFieldWithLocation : GraphQLField
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFieldWithComment : GraphQLField
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLFieldFull : GraphQLField
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
