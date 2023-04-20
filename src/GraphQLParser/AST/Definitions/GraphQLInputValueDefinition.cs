namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputValueDefinition"/>.
/// </summary>
public class GraphQLInputValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasDefaultValueNode
{
    internal GraphQLInputValueDefinition()
    {
        Type = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInputValueDefinition"/>.
    /// </summary>
    public GraphQLInputValueDefinition(GraphQLName name, GraphQLType type)
        : base(name)
    {
        Type = type;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InputValueDefinition;

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with input value type.
    /// </summary>
    public GraphQLType Type { get; set; }

    /// <inheritdoc />
    public GraphQLValue? DefaultValue { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc />
    public override bool IsChildDefinition => true;
}

internal sealed class GraphQLInputValueDefinitionWithLocation : GraphQLInputValueDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLInputValueDefinitionWithComment : GraphQLInputValueDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLInputValueDefinitionFull : GraphQLInputValueDefinition
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
