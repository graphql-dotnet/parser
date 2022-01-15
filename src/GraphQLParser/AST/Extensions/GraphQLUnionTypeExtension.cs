namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.UnionTypeExtension"/>.
/// </summary>
public class GraphQLUnionTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.UnionTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    public GraphQLUnionMemberTypes? Types { get; set; }
}

internal sealed class GraphQLUnionTypeExtensionWithLocation : GraphQLUnionTypeExtension
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLUnionTypeExtensionWithComment : GraphQLUnionTypeExtension
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLUnionTypeExtensionFull : GraphQLUnionTypeExtension
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
