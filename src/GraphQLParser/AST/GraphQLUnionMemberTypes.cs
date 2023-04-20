namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.UnionMemberTypes"/>.
/// </summary>
public class GraphQLUnionMemberTypes : ASTListNode<GraphQLNamedType>
{
    internal GraphQLUnionMemberTypes()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLUnionMemberTypes"/>.
    /// </summary>
    public GraphQLUnionMemberTypes(List<GraphQLNamedType> items)
        : base(items)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.UnionMemberTypes;
}

internal sealed class GraphQLUnionMemberTypesWithLocation : GraphQLUnionMemberTypes
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLUnionMemberTypesWithComment : GraphQLUnionMemberTypes
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLUnionMemberTypesFull : GraphQLUnionMemberTypes
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
