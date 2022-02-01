using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FieldDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLFieldDefinition: {Name}")]
public class GraphQLFieldDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasArgumentsDefinitionNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FieldDefinition;

    /// <summary>
    /// Arguments for this field definition.
    /// </summary>
    public GraphQLArgumentsDefinition? Arguments { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with field type.
    /// </summary>
    public GraphQLType Type { get; set; } = null!;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLFieldDefinitionWithLocation : GraphQLFieldDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFieldDefinitionWithComment : GraphQLFieldDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLFieldDefinitionFull : GraphQLFieldDefinition
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
