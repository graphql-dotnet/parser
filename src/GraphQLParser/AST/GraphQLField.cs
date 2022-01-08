namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Field"/>.
/// </summary>
public class GraphQLField : ASTNode, IHasDirectivesNode, IHasArgumentsNode, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Field;

    /// <summary>
    /// Nested <see cref="GraphQLAlias"/> AST node with field alias (if any).
    /// </summary>
    public GraphQLAlias? Alias { get; set; }

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;

    /// <summary>
    /// Arguments for this field.
    /// </summary>
    public GraphQLArguments? Arguments { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    public GraphQLSelectionSet? SelectionSet { get; set; }
}

internal sealed class GraphQLFieldWithLocation : GraphQLField
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFieldWithComment : GraphQLField
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLFieldFull : GraphQLField
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
