namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Variable"/>.
/// </summary>
public class GraphQLVariable : GraphQLValue, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Variable;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;

    /// <inheritdoc />
    public override object? ClrValue => Name.StringValue;
}

internal sealed class GraphQLVariableWithLocation : GraphQLVariable
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLVariableWithComment : GraphQLVariable
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLVariableFull : GraphQLVariable
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
