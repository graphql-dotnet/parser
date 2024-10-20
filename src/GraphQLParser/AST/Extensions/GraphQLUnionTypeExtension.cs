namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.UnionTypeExtension"/>.
/// </summary>
public class GraphQLUnionTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    internal GraphQLUnionTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLUnionTypeExtension"/>.
    /// </summary>
    public GraphQLUnionTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.UnionTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLUnionMemberTypes"/> AST node with types contained in this union AST node.
    /// </summary>
    public GraphQLUnionMemberTypes? Types { get; set; }
}

internal sealed class GraphQLUnionTypeExtensionWithLocation : GraphQLUnionTypeExtension
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLUnionTypeExtensionWithComment : GraphQLUnionTypeExtension
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLUnionTypeExtensionFull : GraphQLUnionTypeExtension
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
