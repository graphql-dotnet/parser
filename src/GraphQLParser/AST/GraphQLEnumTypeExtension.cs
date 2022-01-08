namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumTypeExtension"/>.
/// </summary>
public class GraphQLEnumTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
{
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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLEnumTypeExtensionWithComment : GraphQLEnumTypeExtension
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLEnumTypeExtensionFull : GraphQLEnumTypeExtension
{
    private GraphQLLocation _location;
    private GraphQLComment? _comment;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}
