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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLSchemaDefinitionWithComment : GraphQLSchemaDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLSchemaDefinitionFull : GraphQLSchemaDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
