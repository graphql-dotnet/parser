using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ListType"/>.
/// </summary>
[DebuggerDisplay("GraphQLListType: [{Type}]")]
public class GraphQLListType : GraphQLType
{
    internal GraphQLListType()
    {
        Type = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLListType"/>.
    /// </summary>
    public GraphQLListType(GraphQLType type)
    {
        Type = type;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ListType;

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with wrapped type.
    /// </summary>
    public GraphQLType Type { get; set; }
}

internal sealed class GraphQLListTypeWithLocation : GraphQLListType
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLListTypeWithComment : GraphQLListType
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLListTypeFull : GraphQLListType
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
