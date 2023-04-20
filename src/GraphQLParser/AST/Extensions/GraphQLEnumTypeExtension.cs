namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumTypeExtension"/>.
/// </summary>
public class GraphQLEnumTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    internal GraphQLEnumTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLEnumTypeExtension"/>.
    /// </summary>
    public GraphQLEnumTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.EnumTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLEnumValuesDefinition"/> AST node with enum values.
    /// </summary>
    public GraphQLEnumValuesDefinition? Values { get; set; }
}

internal sealed class GraphQLEnumTypeExtensionWithLocation : GraphQLEnumTypeExtension
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLEnumTypeExtensionWithComment : GraphQLEnumTypeExtension
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLEnumTypeExtensionFull : GraphQLEnumTypeExtension
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
