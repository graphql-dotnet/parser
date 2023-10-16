namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.VariablesDefinition"/>.
/// </summary>
public class GraphQLVariablesDefinition : ASTListNode<GraphQLVariableDefinition>
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.VariablesDefinition;
}

internal sealed class GraphQLVariablesDefinitionWithLocation : GraphQLVariablesDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLVariablesDefinitionWithComment : GraphQLVariablesDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLVariablesDefinitionFull : GraphQLVariablesDefinition
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
