namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InterfaceTypeExtension"/>.
/// </summary>
public class GraphQLInterfaceTypeExtension : GraphQLTypeExtension, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
{
    /// <summary>Initializes a new instance.</summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLInterfaceTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInterfaceTypeExtension"/>.
    /// </summary>
    public GraphQLInterfaceTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InterfaceTypeExtension;

    /// <inheritdoc/>
    public GraphQLImplementsInterfaces? Interfaces { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
    public GraphQLFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLInterfaceTypeExtensionWithLocation : GraphQLInterfaceTypeExtension
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLInterfaceTypeExtensionWithComment : GraphQLInterfaceTypeExtension
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLInterfaceTypeExtensionFull : GraphQLInterfaceTypeExtension
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
