namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputObjectTypeExtension"/>.
/// </summary>
public class GraphQLInputObjectTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    internal GraphQLInputObjectTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInputObjectTypeExtension"/>.
    /// </summary>
    public GraphQLInputObjectTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLInputFieldsDefinition"/> AST node with input fields definition of this AST node.
    /// </summary>
    public GraphQLInputFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLInputObjectTypeExtensionWithLocation : GraphQLInputObjectTypeExtension
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLInputObjectTypeExtensionWithComment : GraphQLInputObjectTypeExtension
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLInputObjectTypeExtensionFull : GraphQLInputObjectTypeExtension
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
