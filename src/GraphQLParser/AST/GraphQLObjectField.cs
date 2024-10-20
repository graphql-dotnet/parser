namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectField"/>.
/// </summary>
public class GraphQLObjectField : ASTNode, INamedNode
{
    internal GraphQLObjectField()
    {
        Name = null!;
        Value = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLObjectField"/>.
    /// </summary>
    public GraphQLObjectField(GraphQLName name, GraphQLValue value)
    {
        Name = name;
        Value = value;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ObjectField;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }

    /// <summary>
    /// Value of the field represented as a nested AST node.
    /// </summary>
    public GraphQLValue Value { get; set; }
}

internal sealed class GraphQLObjectFieldWithLocation : GraphQLObjectField
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLObjectFieldWithComment : GraphQLObjectField
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLObjectFieldFull : GraphQLObjectField
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
