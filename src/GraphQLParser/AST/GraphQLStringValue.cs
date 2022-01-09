namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.StringValue"/>.
/// </summary>
public class GraphQLStringValue : GraphQLValue
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.StringValue;

    /// <summary>
    /// String value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; set; }
}

internal sealed class GraphQLStringValueWithLocation : GraphQLStringValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLStringValueWithComment : GraphQLStringValue
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLStringValueFull : GraphQLStringValue
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
