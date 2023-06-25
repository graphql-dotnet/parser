using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.NamedType"/>.
/// </summary>
[DebuggerDisplay("GraphQLNamedType: {Name}")]
public class GraphQLNamedType : GraphQLType, INamedNode
{
    /// <summary>Initializes a new instance.</summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLNamedType()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLNamedType"/>.
    /// </summary>
    public GraphQLNamedType(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.NamedType;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }
}

internal sealed class GraphQLNamedTypeWithLocation : GraphQLNamedType
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLNamedTypeWithComment : GraphQLNamedType
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLNamedTypeFull : GraphQLNamedType
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
