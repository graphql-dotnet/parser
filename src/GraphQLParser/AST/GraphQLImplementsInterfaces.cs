namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ImplementsInterfaces"/>.
/// </summary>
public class GraphQLImplementsInterfaces : ASTListNode<GraphQLNamedType>
{
    internal GraphQLImplementsInterfaces()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLImplementsInterfaces"/>.
    /// </summary>
    public GraphQLImplementsInterfaces(List<GraphQLNamedType> items)
        : base(items)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ImplementsInterfaces;
}

internal sealed class GraphQLImplementsInterfacesWithLocation : GraphQLImplementsInterfaces
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLImplementsInterfacesWithComment : GraphQLImplementsInterfaces
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLImplementsInterfacesFull : GraphQLImplementsInterfaces
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
