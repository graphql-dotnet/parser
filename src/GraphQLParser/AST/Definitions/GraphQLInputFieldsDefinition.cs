namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputFieldsDefinition"/>.
/// </summary>
public class GraphQLInputFieldsDefinition : ASTListNode<GraphQLInputValueDefinition>
{
    internal GraphQLInputFieldsDefinition()
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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLInputFieldsDefinitionWithComment : GraphQLInputFieldsDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLInputFieldsDefinitionFull : GraphQLInputFieldsDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
