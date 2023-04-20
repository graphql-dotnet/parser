namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.UnionTypeExtension"/>.
/// </summary>
public class GraphQLUnionTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    internal GraphQLUnionTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLUnionTypeExtension"/>.
    /// </summary>
    public GraphQLUnionTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.UnionTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLUnionMemberTypes"/> AST node with types contained in this union AST node.
    /// </summary>
    public GraphQLUnionMemberTypes? Types { get; set; }
}

internal sealed class GraphQLUnionTypeExtensionWithLocation : GraphQLUnionTypeExtension
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLUnionTypeExtensionWithComment : GraphQLUnionTypeExtension
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLUnionTypeExtensionFull : GraphQLUnionTypeExtension
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
