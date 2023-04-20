using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputObjectTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLInputObjectTypeDefinition: {Name}")]
public class GraphQLInputObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    /// <summary>Initializes a new instance.</summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLInputObjectTypeDefinition()
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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLInputObjectTypeDefinitionWithComment : GraphQLInputObjectTypeDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLInputObjectTypeDefinitionFull : GraphQLInputObjectTypeDefinition
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
