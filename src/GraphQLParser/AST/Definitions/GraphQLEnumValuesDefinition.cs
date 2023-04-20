namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumValuesDefinition"/>.
/// </summary>
public class GraphQLEnumValuesDefinition : ASTListNode<GraphQLEnumValueDefinition>
{
    internal GraphQLEnumValuesDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLEnumValuesDefinition"/>.
    /// </summary>
    public GraphQLEnumValuesDefinition(List<GraphQLEnumValueDefinition> items)
        : base(items)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.EnumValuesDefinition;
}

internal sealed class GraphQLEnumValuesDefinitionWithLocation : GraphQLEnumValuesDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLEnumValuesDefinitionWithComment : GraphQLEnumValuesDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLEnumValuesDefinitionFull : GraphQLEnumValuesDefinition
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
