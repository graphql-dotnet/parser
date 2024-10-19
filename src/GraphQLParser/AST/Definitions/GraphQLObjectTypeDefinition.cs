using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLObjectTypeDefinition: {Name}")]
public class GraphQLObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
{
    internal GraphQLObjectTypeDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLObjectTypeDefinition"/>.
    /// </summary>
    public GraphQLObjectTypeDefinition(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ObjectTypeDefinition;

    /// <inheritdoc />
    public GraphQLImplementsInterfaces? Interfaces { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc/>
    public GraphQLFieldsDefinition? Fields { get; set; }
}

internal sealed class GraphQLObjectTypeDefinitionWithLocation : GraphQLObjectTypeDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLObjectTypeDefinitionWithComment : GraphQLObjectTypeDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLObjectTypeDefinitionFull : GraphQLObjectTypeDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
