namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.VariablesDefinition"/>.
/// </summary>
public class GraphQLVariablesDefinition : ASTListNode<GraphQLVariableDefinition>
{
    internal GraphQLVariablesDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLVariablesDefinition"/>.
    /// </summary>
    public GraphQLVariablesDefinition(List<GraphQLVariableDefinition> items)
        : base(items)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.VariablesDefinition;
}

internal sealed class GraphQLVariablesDefinitionWithLocation : GraphQLVariablesDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLVariablesDefinitionWithComment : GraphQLVariablesDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLVariablesDefinitionFull : GraphQLVariablesDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
