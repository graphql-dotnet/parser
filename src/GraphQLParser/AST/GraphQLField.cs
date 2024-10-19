namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Field"/>.
/// </summary>
public class GraphQLField : ASTNode, ISelectionNode, IHasSelectionSetNode, IHasDirectivesNode, IHasArgumentsNode, INamedNode
{
    internal GraphQLField()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLField"/>.
    /// </summary>
    public GraphQLField(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Field;

    /// <summary>
    /// Nested <see cref="GraphQLAlias"/> AST node with field alias (if any).
    /// </summary>
    public GraphQLAlias? Alias { get; set; }

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }

    /// <summary>
    /// Arguments for this field.
    /// </summary>
    public GraphQLArguments? Arguments { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
    public GraphQLSelectionSet? SelectionSet { get; set; }
}

internal sealed class GraphQLFieldWithLocation : GraphQLField
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLFieldWithComment : GraphQLField
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLFieldFull : GraphQLField
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
