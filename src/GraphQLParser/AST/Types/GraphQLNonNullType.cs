using System.Collections.Generic;
using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.NonNullType"/>.
/// </summary>
[DebuggerDisplay("GraphQLNonNullType: {Type}!")]
public class GraphQLNonNullType : GraphQLType
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.NonNullType;

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with wrapped type.
    /// </summary>
    public GraphQLType Type { get; set; } = null!;
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
