namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectField"/>.
/// </summary>
public class GraphQLObjectField : ASTNode, INamedNode
{
    internal GraphQLObjectField()
    {
        Name = null!;
        Value = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLObjectField"/>.
    /// </summary>
    public GraphQLObjectField(GraphQLName name, GraphQLValue value)
    {
        Name = name;
        Value = value;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ObjectField;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }

    /// <summary>
    /// Value of the field represented as a nested AST node.
    /// </summary>
    public GraphQLValue Value { get; set; }
}

internal sealed class GraphQLObjectFieldWithLocation : GraphQLObjectField
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLObjectFieldWithComment : GraphQLObjectField
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLObjectFieldFull : GraphQLObjectField
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
