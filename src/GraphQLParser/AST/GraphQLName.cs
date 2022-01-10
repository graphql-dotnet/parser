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
