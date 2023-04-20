namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Document"/>.
/// </summary>
public class GraphQLDocument : ASTNode
{
    internal GraphQLDocument()
    {
        Definitions = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLDocument"/>.
    /// </summary>
    public GraphQLDocument(List<ASTNode> definitions)
    {
        Definitions = definitions;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Document;

    /// <summary>
    /// All definitions in this document represented as a list of nested AST nodes.
    /// </summary>
    public List<ASTNode> Definitions { get; set; }

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
