using System.Collections.Generic;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FragmentSpread"/>.
/// </summary>
public class GraphQLFragmentSpread : ASTNode, ISelectionNode, IHasDirectivesNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FragmentSpread;

    /// <summary>
    /// Fragment name represented as a nested AST node.
    /// </summary>
    public GraphQLFragmentName FragmentName { get; set; } = null!;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLFragmentSpreadWithLocation : GraphQLFragmentSpread
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFragmentSpreadWithComment : GraphQLFragmentSpread
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLFragmentSpreadFull : GraphQLFragmentSpread
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
