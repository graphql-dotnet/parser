namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumValueDefinition"/>.
/// </summary>
public class GraphQLEnumValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.EnumValueDefinition;

    /// <summary>
    /// Enum value represented as a nested AST node. Alas, inherited
    /// <see cref="GraphQLTypeDefinition.Name"/> property holds almost
    /// the same data and should be set as well.
    /// </summary>
    public GraphQLEnumValue EnumValue { get; set; } = null!;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLEnumValueDefinitionWithLocation : GraphQLEnumValueDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLEnumValueDefinitionWithComment : GraphQLEnumValueDefinition
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLEnumValueDefinitionFull : GraphQLEnumValueDefinition
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
