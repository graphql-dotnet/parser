namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectTypeDefinition"/>.
/// </summary>
public class GraphQLObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ObjectTypeDefinition;

    /// <inheritdoc />
    public GraphQLImplementsInterfaces? Interfaces { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
    public GraphQLFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLObjectTypeDefinitionWithLocation : GraphQLObjectTypeDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLObjectTypeDefinitionWithComment : GraphQLObjectTypeDefinition
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLObjectTypeDefinitionFull : GraphQLObjectTypeDefinition
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
