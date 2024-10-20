using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.NonNullType"/>.
/// </summary>
[DebuggerDisplay("GraphQLNonNullType: {Type}!")]
public class GraphQLNonNullType : GraphQLType
{
    internal GraphQLNonNullType()
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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLNonNullTypeWithComment : GraphQLNonNullType
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLNonNullTypeFull : GraphQLNonNullType
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
