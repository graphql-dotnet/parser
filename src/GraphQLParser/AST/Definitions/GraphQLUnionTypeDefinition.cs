namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.UnionTypeDefinition"/>.
/// </summary>
public class GraphQLUnionTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.UnionTypeDefinition;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    public GraphQLUnionMemberTypes? Types { get; set; }
}

internal sealed class GraphQLUnionTypeDefinitionWithLocation : GraphQLUnionTypeDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLUnionTypeDefinitionWithComment : GraphQLUnionTypeDefinition
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLUnionTypeDefinitionFull : GraphQLUnionTypeDefinition
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
