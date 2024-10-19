namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.RootOperationTypeDefinition"/>.
/// </summary>
public class GraphQLRootOperationTypeDefinition : ASTNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.RootOperationTypeDefinition;

    /// <summary>
    /// Kind of operation: query, mutation or subscription.
    /// </summary>
    public OperationType Operation { get; set; }

    /// <summary>
    /// Type of this root operation.
    /// </summary>
    public GraphQLNamedType? Type { get; set; }
}

internal sealed class GraphQLRootOperationTypeDefinitionWithLocation : GraphQLRootOperationTypeDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLRootOperationTypeDefinitionWithComment : GraphQLRootOperationTypeDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLRootOperationTypeDefinitionFull : GraphQLRootOperationTypeDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
