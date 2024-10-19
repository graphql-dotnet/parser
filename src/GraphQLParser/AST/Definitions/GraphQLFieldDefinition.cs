using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FieldDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLFieldDefinition: {Name}")]
public class GraphQLFieldDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasArgumentsDefinitionNode
{
    internal GraphQLFieldDefinition()
    {
        Type = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLFieldDefinition"/>.
    /// </summary>
    public GraphQLFieldDefinition(GraphQLName name, GraphQLType type)
        : base(name)
    {
        Type = type;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FieldDefinition;

    /// <summary>
    /// Arguments for this field definition.
    /// </summary>
    public GraphQLArgumentsDefinition? Arguments { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with field type.
    /// </summary>
    public GraphQLType Type { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc />
    public override bool IsChildDefinition => true;
}

internal sealed class GraphQLFieldDefinitionWithLocation : GraphQLFieldDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLFieldDefinitionWithComment : GraphQLFieldDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLFieldDefinitionFull : GraphQLFieldDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
