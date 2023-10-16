namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ImplementsInterfaces"/>.
/// </summary>
public class GraphQLImplementsInterfaces : ASTListNode<GraphQLNamedType>
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ImplementsInterfaces;
}

internal sealed class GraphQLImplementsInterfacesWithLocation : GraphQLImplementsInterfaces
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLImplementsInterfacesWithComment : GraphQLImplementsInterfaces
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLImplementsInterfacesFull : GraphQLImplementsInterfaces
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
