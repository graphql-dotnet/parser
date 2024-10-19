namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectTypeExtension"/>.
/// </summary>
public class GraphQLObjectTypeExtension : GraphQLTypeExtension, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
{
    internal GraphQLObjectTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLObjectTypeExtension"/>.
    /// </summary>
    public GraphQLObjectTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ObjectTypeExtension;

    /// <inheritdoc/>
    public GraphQLImplementsInterfaces? Interfaces { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
    public GraphQLFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLObjectTypeExtensionWithLocation : GraphQLObjectTypeExtension
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLObjectTypeExtensionWithComment : GraphQLObjectTypeExtension
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLObjectTypeExtensionFull : GraphQLObjectTypeExtension
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
