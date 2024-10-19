namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumTypeExtension"/>.
/// </summary>
public class GraphQLEnumTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
    internal GraphQLEnumTypeExtension()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLEnumTypeExtension"/>.
    /// </summary>
    public GraphQLEnumTypeExtension(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.EnumTypeExtension;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLEnumValuesDefinition"/> AST node with enum values.
    /// </summary>
    public GraphQLEnumValuesDefinition? Values { get; set; }
}

internal sealed class GraphQLEnumTypeExtensionWithLocation : GraphQLEnumTypeExtension
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLEnumTypeExtensionWithComment : GraphQLEnumTypeExtension
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLEnumTypeExtensionFull : GraphQLEnumTypeExtension
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
