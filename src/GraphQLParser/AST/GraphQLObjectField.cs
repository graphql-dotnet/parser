namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectField"/>.
/// </summary>
public class GraphQLObjectField : ASTNode, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ObjectField;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;

    /// <summary>
    /// Value of the field represented as a nested AST node.
    /// </summary>
    public GraphQLValue Value { get; set; } = null!;
}

internal sealed class GraphQLObjectFieldWithLocation : GraphQLObjectField
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLObjectFieldWithComment : GraphQLObjectField
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLObjectFieldFull : GraphQLObjectField
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
