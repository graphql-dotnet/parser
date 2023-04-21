namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Arguments"/>.
/// </summary>
public class GraphQLArguments : ASTListNode<GraphQLArgument>
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLArguments"/>.
    /// </summary>
    public GraphQLArguments()
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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLArgumentsWithComment : GraphQLArguments
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLArgumentsFull : GraphQLArguments
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
