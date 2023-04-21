namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputObjectTypeExtension"/>.
/// </summary>
public class GraphQLInputObjectTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInputObjectTypeExtension"/>.
    /// </summary>
    public GraphQLInputObjectTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInputObjectTypeExtension"/>.
    /// </summary>
    public GraphQLInputObjectTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLInputFieldsDefinition"/> AST node with input fields definition of this AST node.
    /// </summary>
    public GraphQLInputFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLInputObjectTypeExtensionWithLocation : GraphQLInputObjectTypeExtension
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLInputObjectTypeExtensionWithComment : GraphQLInputObjectTypeExtension
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLInputObjectTypeExtensionFull : GraphQLInputObjectTypeExtension
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
