namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.SchemaExtension"/>.
/// </summary>
public class GraphQLSchemaExtension : ASTNode, IHasDirectivesNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.SchemaExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Root operation type definitions added by this schema extension represented as a list of nested AST nodes.
    /// </summary>
    public List<GraphQLRootOperationTypeDefinition>? OperationTypes { get; set; }
}

internal sealed class GraphQLSchemaExtensionWithLocation : GraphQLSchemaExtension
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLSchemaExtensionWithComment : GraphQLSchemaExtension
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLSchemaExtensionFull : GraphQLSchemaExtension
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
