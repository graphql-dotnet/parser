using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Comment"/>.
/// </summary>
[DebuggerDisplay("GraphQLComment: {Value}")]
public class GraphQLComment : ASTNode, IHasValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Comment;

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLComment(ROM value)
    {
        Value = value;
    }

    /// <summary>
    /// Comment value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; internal set; }
}

internal sealed class GraphQLCommentWithLocation : GraphQLComment
{
    public override GraphQLLocation Location { get; set; }

    /// <inheritdoc cref="GraphQLComment(ROM)"/>
    public GraphQLCommentWithLocation(ROM value)
        : base(value)
    {
    }
}
