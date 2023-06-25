namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputFieldsDefinition"/>.
/// </summary>
public class GraphQLInputFieldsDefinition : ASTListNode<GraphQLInputValueDefinition>
{
    /// <summary>Initializes a new instance.</summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLInputFieldsDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInputFieldsDefinition"/>.
    /// </summary>
    public GraphQLInputFieldsDefinition(List<GraphQLInputValueDefinition> items)
        : base(items)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InputFieldsDefinition;
}

internal sealed class GraphQLInputFieldsDefinitionWithLocation : GraphQLInputFieldsDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLInputFieldsDefinitionWithComment : GraphQLInputFieldsDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLInputFieldsDefinitionFull : GraphQLInputFieldsDefinition
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
