namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Argument"/>.
/// </summary>
public class GraphQLArgument : ASTNode, INamedNode
{
    internal GraphQLArgument()
    {
        Name = null!;
        Value = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLArgument"/>.
    /// </summary>
    public GraphQLArgument(GraphQLName name, GraphQLValue value)
    {
        Name = name;
        Value = value;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Argument;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLValue"/> AST node with argument value.
    /// </summary>
    public GraphQLValue Value { get; set; }
}

internal sealed class GraphQLArgumentWithLocation : GraphQLArgument
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLArgumentWithComment : GraphQLArgument
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLArgumentFull : GraphQLArgument
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
