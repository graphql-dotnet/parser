using System.Collections.Generic;

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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLSchemaExtensionWithComment : GraphQLSchemaExtension
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLSchemaExtensionFull : GraphQLSchemaExtension
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
