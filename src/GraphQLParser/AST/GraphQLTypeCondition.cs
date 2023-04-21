namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.TypeCondition"/>.
/// </summary>
public class GraphQLTypeCondition : ASTNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLTypeCondition"/>.
    /// </summary>
    public GraphQLTypeCondition()
    {
        Type = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLTypeCondition"/>.
    /// </summary>
    public GraphQLTypeCondition(GraphQLNamedType type)
    {
        Type = type;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.TypeCondition;

    /// <summary>
    /// Type to which this condition is applied.
    /// </summary>
    public GraphQLNamedType Type { get; set; }
}

internal sealed class GraphQLTypeConditionWithLocation : GraphQLTypeCondition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLTypeConditionWithComment : GraphQLTypeCondition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLTypeConditionFull : GraphQLTypeCondition
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
