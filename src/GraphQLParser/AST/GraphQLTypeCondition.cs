namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.TypeCondition"/>.
/// </summary>
public class GraphQLTypeCondition : ASTNode
{
    internal GraphQLTypeCondition()
    {
        Type = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLTypeCondition"/>.
    /// </summary>
    public GraphQLTypeCondition(GraphQLNamedType type)
    {
        Type = type;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.TypeCondition;

    /// <summary>
    /// Type to which this condition is applied.
    /// </summary>
    public GraphQLNamedType Type { get; set; }
}

internal sealed class GraphQLTypeConditionWithLocation : GraphQLTypeCondition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLTypeConditionWithComment : GraphQLTypeCondition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLTypeConditionFull : GraphQLTypeCondition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
