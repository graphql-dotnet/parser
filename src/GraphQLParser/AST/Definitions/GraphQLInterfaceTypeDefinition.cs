using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InterfaceTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLInterfaceTypeDefinition: {Name}")]
public class GraphQLInterfaceTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
{
    internal GraphQLInterfaceTypeDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInterfaceTypeDefinition"/>.
    /// </summary>
    public GraphQLInterfaceTypeDefinition(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InterfaceTypeDefinition;

    /// <inheritdoc/>
    public GraphQLImplementsInterfaces? Interfaces { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
    public GraphQLFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLInterfaceTypeDefinitionWithLocation : GraphQLInterfaceTypeDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLInterfaceTypeDefinitionWithComment : GraphQLInterfaceTypeDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLInterfaceTypeDefinitionFull : GraphQLInterfaceTypeDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
