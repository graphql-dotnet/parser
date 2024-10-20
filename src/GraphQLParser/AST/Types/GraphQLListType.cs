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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLListTypeWithComment : GraphQLListType
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLListTypeFull : GraphQLListType
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
