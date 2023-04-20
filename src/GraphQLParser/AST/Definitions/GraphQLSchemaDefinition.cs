namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.SchemaDefinition"/>.
/// </summary>
public class GraphQLSchemaDefinition : ASTNode, IHasDirectivesNode, IHasDescriptionNode
{
    internal GraphQLSchemaDefinition()
    {
        OperationTypes = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLSchemaDefinition"/>.
    /// </summary>
    public GraphQLSchemaDefinition(List<GraphQLRootOperationTypeDefinition> operationTypes)
    {
        OperationTypes = operationTypes;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.SchemaDefinition;

    /// <inheritdoc/>
    public GraphQLDescription? Description { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// All root operation type definitions in this schema represented as a list of nested AST nodes.
    /// </summary>
    public List<GraphQLRootOperationTypeDefinition> OperationTypes { get; set; }
    //TODO: https://github.com/graphql/graphql-spec/issues/921
}

internal sealed class GraphQLSchemaDefinitionWithLocation : GraphQLSchemaDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLSchemaDefinitionWithComment : GraphQLSchemaDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLSchemaDefinitionFull : GraphQLSchemaDefinition
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
