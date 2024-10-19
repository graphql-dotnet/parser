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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLEnumValuesDefinitionWithComment : GraphQLEnumValuesDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLEnumValuesDefinitionFull : GraphQLEnumValuesDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
