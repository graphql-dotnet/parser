using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ScalarTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLScalarTypeDefinition: {Name}")]
public class GraphQLScalarTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLScalarTypeDefinition"/>.
    /// </summary>
    public GraphQLScalarTypeDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLScalarTypeDefinition"/>.
    /// </summary>
    public GraphQLScalarTypeDefinition(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ScalarTypeDefinition;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLScalarTypeDefinitionWithLocation : GraphQLScalarTypeDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLScalarTypeDefinitionWithComment : GraphQLScalarTypeDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLScalarTypeDefinitionFull : GraphQLScalarTypeDefinition
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
