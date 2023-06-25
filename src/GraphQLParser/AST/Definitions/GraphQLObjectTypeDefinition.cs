using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLObjectTypeDefinition: {Name}")]
public class GraphQLObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLObjectTypeDefinition"/>.
    /// </summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLObjectTypeDefinition()
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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLObjectTypeDefinitionWithComment : GraphQLObjectTypeDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLObjectTypeDefinitionFull : GraphQLObjectTypeDefinition
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
