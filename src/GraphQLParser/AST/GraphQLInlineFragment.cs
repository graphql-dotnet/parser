namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InlineFragment"/>.
/// </summary>
public class GraphQLInlineFragment : ASTNode, ISelectionNode, IHasSelectionSetNode, IHasDirectivesNode
{
    internal GraphQLInlineFragment()
    {
        SelectionSet = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInlineFragment"/>.
    /// </summary>
    public GraphQLInlineFragment(GraphQLSelectionSet selectionSet)
    {
        SelectionSet = selectionSet;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InlineFragment;

    /// <summary>
    /// Nested <see cref="GraphQLTypeCondition"/> AST node with type condition of this inline fragment.
    /// If <see langword="null"/>, an inline fragment is considered to be of the same type as the enclosing context.
    /// </summary>
    public GraphQLTypeCondition? TypeCondition { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
#pragma warning disable CS8767 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public GraphQLSelectionSet SelectionSet { get; set; }
#pragma warning restore CS8767
}

internal sealed class GraphQLInlineFragmentWithLocation : GraphQLInlineFragment
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLInlineFragmentWithComment : GraphQLInlineFragment
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLInlineFragmentFull : GraphQLInlineFragment
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
