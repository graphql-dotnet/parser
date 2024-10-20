using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputObjectTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLInputObjectTypeDefinition: {Name}")]
public class GraphQLInputObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    internal GraphQLInputObjectTypeDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInputObjectTypeDefinition"/>.
    /// </summary>
    public GraphQLInputObjectTypeDefinition(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeDefinition;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLInputFieldsDefinition"/> AST node with input fields definition of this AST node.
    /// </summary>
    public GraphQLInputFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLInputObjectTypeDefinitionWithLocation : GraphQLInputObjectTypeDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLInputObjectTypeDefinitionWithComment : GraphQLInputObjectTypeDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLInputObjectTypeDefinitionFull : GraphQLInputObjectTypeDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
