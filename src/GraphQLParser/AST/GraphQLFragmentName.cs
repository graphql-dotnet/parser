using System.Collections.Generic;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FragmentName"/>.
/// </summary>
public class GraphQLFragmentName : ASTNode, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FragmentName;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;
}

internal sealed class GraphQLFragmentNameWithLocation : GraphQLFragmentName
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFragmentNameWithComment : GraphQLFragmentName
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLFragmentNameFull : GraphQLFragmentName
{
    private GraphQLLocation _location;
    private List<GraphQLComment>? _comments;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}
