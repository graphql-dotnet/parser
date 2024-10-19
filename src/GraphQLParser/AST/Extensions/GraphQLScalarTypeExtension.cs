namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ScalarTypeExtension"/>.
/// </summary>
public class GraphQLScalarTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    internal GraphQLScalarTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLScalarTypeExtension"/>.
    /// </summary>
    public GraphQLScalarTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ScalarTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLScalarTypeExtensionWithLocation : GraphQLScalarTypeExtension
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLScalarTypeExtensionWithComment : GraphQLScalarTypeExtension
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLScalarTypeExtensionFull : GraphQLScalarTypeExtension
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
