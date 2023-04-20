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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLInterfaceTypeDefinitionWithComment : GraphQLInterfaceTypeDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLInterfaceTypeDefinitionFull : GraphQLInterfaceTypeDefinition
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
