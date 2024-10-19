namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FieldsDefinition"/>.
/// </summary>
public class GraphQLFieldsDefinition : ASTListNode<GraphQLFieldDefinition>
{
    internal GraphQLFieldsDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLFieldsDefinition"/>.
    /// </summary>
    public GraphQLFieldsDefinition(List<GraphQLFieldDefinition> items)
        : base(items)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FieldsDefinition;
}

internal sealed class GraphQLFieldsDefinitionWithLocation : GraphQLFieldsDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLFieldsDefinitionWithComment : GraphQLFieldsDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLFieldsDefinitionFull : GraphQLFieldsDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
