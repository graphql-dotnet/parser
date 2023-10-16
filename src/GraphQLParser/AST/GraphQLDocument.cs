namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Document"/>.
/// </summary>
public class GraphQLDocument : ASTNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Document;

    /// <summary>
    /// All definitions in this document represented as a list of nested AST nodes.
    /// </summary>
    public List<ASTNode> Definitions { get; set; } = null!;

    /// <summary>
    /// Comments that have not been correlated to any AST node of GraphQL document.
    /// </summary>
    public List<List<GraphQLComment>>? UnattachedComments { get; set; }

    /// <summary>
    /// Input data from which this document was built.
    /// </summary>
    public ROM Source { get; set; }
}

internal sealed class GraphQLDocumentWithLocation : GraphQLDocument
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}
