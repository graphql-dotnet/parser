namespace GraphQLParser.AST;

/// <summary>
/// Represents a single node in the GraphQL document AST (Abstract Syntax Tree).
/// </summary>
public abstract class ASTNode
{
    /// <summary>
    /// Kind of this node.
    /// </summary>
    public abstract ASTNodeKind Kind { get; }

    /// <summary>
    /// Location of a node within a document's original text.
    /// </summary>
    public virtual GraphQLLocation Location { get => default; set { } }

    /// <summary>
    /// Comments for this node if any.
    /// </summary>
    public virtual GraphQLComment? Comment { get => default; set { } }
}
