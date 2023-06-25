namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ScalarTypeExtension"/>.
/// </summary>
public class GraphQLScalarTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    /// <summary>Initializes a new instance.</summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLScalarTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLScalarTypeExtension"/>.
    /// </summary>
    public GraphQLScalarTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ScalarTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLScalarTypeExtensionWithLocation : GraphQLScalarTypeExtension
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLScalarTypeExtensionWithComment : GraphQLScalarTypeExtension
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLScalarTypeExtensionFull : GraphQLScalarTypeExtension
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
