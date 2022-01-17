namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Arguments"/>.
/// </summary>
public class GraphQLArguments : ASTListNode<GraphQLArgument>
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Arguments;

    /// <summary>
    /// Searches arguments for the first matching agrument by its name,
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
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLArgumentsFull : GraphQLArguments
{
    private GraphQLLocation _location;
    private GraphQLComment? _comment;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}
