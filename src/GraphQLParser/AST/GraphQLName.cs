using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Name"/>.
/// </summary>
[DebuggerDisplay("GraphQLName: {Value}")]
public class GraphQLName : ASTNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Name;

    /// <summary>
    /// Name value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; set; }

    /// <summary>
    /// Implicitly casts <see cref="GraphQLName"/> to <see cref="ROM"/>.
    /// </summary>
    public static implicit operator ROM(GraphQLName node) => node.Value;

    /// <summary>
    /// Explicitly casts <see cref="GraphQLName"/> to <see cref="string"/>.
    /// </summary>
    public static explicit operator string(GraphQLName node) => (string)node.Value;
}

internal sealed class GraphQLNameWithLocation : GraphQLName
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLNameWithComment : GraphQLName
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLNameFull : GraphQLName
{
    private GraphQLLocation _location;
    private GraphQLComment? _comment;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}
