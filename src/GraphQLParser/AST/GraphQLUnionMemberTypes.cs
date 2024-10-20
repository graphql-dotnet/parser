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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLUnionMemberTypesWithComment : GraphQLUnionMemberTypes
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLUnionMemberTypesFull : GraphQLUnionMemberTypes
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
