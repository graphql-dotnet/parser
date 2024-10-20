namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InterfaceTypeExtension"/>.
/// </summary>
public class GraphQLInterfaceTypeExtension : GraphQLTypeExtension, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
{
    internal GraphQLInterfaceTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInterfaceTypeExtension"/>.
    /// </summary>
    public GraphQLInterfaceTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InterfaceTypeExtension;

    /// <inheritdoc/>
    public GraphQLImplementsInterfaces? Interfaces { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
    public GraphQLFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLInterfaceTypeExtensionWithLocation : GraphQLInterfaceTypeExtension
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLInterfaceTypeExtensionWithComment : GraphQLInterfaceTypeExtension
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLInterfaceTypeExtensionFull : GraphQLInterfaceTypeExtension
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
