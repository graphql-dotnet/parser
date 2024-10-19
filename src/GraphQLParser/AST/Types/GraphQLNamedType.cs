using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.NamedType"/>.
/// </summary>
[DebuggerDisplay("GraphQLNamedType: {Name}")]
public class GraphQLNamedType : GraphQLType, INamedNode
{
    internal GraphQLNamedType()
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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLNamedTypeWithComment : GraphQLNamedType
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLNamedTypeFull : GraphQLNamedType
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
