namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Arguments"/>.
/// </summary>
public class GraphQLArguments : ASTListNode<GraphQLArgument>
{
    internal GraphQLArguments()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLArguments"/>.
    /// </summary>
    public GraphQLArguments(List<GraphQLArgument> items)
        : base(items)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Arguments;

    /// <summary>
    /// Searches arguments for the first matching argument by its name,
    /// or returns <see langword="null"/> if none is found.
    /// </summary>
    public GraphQLValue? ValueFor(ROM name)
    {
        if (Items != null)
        {
            foreach (var item in Items)
            {
                if (item.Name == name)
                    return item.Value;
            }
        }

        return null;
    }
}

internal sealed class GraphQLArgumentsWithLocation : GraphQLArguments
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLArgumentsWithComment : GraphQLArguments
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLArgumentsFull : GraphQLArguments
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
