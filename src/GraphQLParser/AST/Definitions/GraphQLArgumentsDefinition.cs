namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ArgumentsDefinition"/>.
/// </summary>
public class GraphQLArgumentsDefinition : ASTListNode<GraphQLInputValueDefinition>
{
    internal GraphQLArgumentsDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLArgumentsDefinition"/>.
    /// </summary>
    public GraphQLArgumentsDefinition(List<GraphQLInputValueDefinition> items)
        : base(items)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ArgumentsDefinition;
}

internal sealed class GraphQLArgumentsDefinitionWithLocation : GraphQLArgumentsDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLArgumentsDefinitionWithComment : GraphQLArgumentsDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLArgumentsDefinitionFull : GraphQLArgumentsDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
