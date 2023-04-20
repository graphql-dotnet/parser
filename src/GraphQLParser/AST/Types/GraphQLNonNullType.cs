using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.NonNullType"/>.
/// </summary>
[DebuggerDisplay("GraphQLNonNullType: {Type}!")]
public class GraphQLNonNullType : GraphQLType
{
    /// <summary>Initializes a new instance.</summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLNonNullType()
    {
        Type = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLNonNullType"/>.
    /// </summary>
    public GraphQLNonNullType(GraphQLType type)
    {
        Type = type;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.NonNullType;

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with wrapped type.
    /// </summary>
    public GraphQLType Type { get; set; }
}

internal sealed class GraphQLNonNullTypeWithLocation : GraphQLNonNullType
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLNonNullTypeWithComment : GraphQLNonNullType
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLNonNullTypeFull : GraphQLNonNullType
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
