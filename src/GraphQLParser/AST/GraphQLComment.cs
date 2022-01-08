using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Comment"/>.
/// </summary>
[DebuggerDisplay("{Text}")]
public class GraphQLComment : ASTNode
{
    //private GraphQLComment? _comment;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Comment;

    /// <summary>
    /// Comment value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Text { get; set; }

    // Comment itself can't have a comment
    //public override GraphQLComment? Comment
    //{
    //    get => _comment;
    //    set => _comment = value;
    //}
}

internal class GraphQLCommentWithLocation : GraphQLComment
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}
