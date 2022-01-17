using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Comment"/>.
/// </summary>
[DebuggerDisplay("GraphQLComment: {Text}")]
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

internal class GraphQLCommentWithLocation : GraphQLComment
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    /// <inheritdoc cref="GraphQLComment(ROM)"/>
    public GraphQLCommentWithLocation(ROM value)
        : base(value)
    {
    }
}
