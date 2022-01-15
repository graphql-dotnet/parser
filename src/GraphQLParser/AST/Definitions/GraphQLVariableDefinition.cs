namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.VariableDefinition"/>.
/// </summary>
public class GraphQLVariableDefinition : ASTNode, IHasDirectivesNode, IHasDefaultValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.VariableDefinition;

    /// <summary>
    /// Nested <see cref="GraphQLVariable"/> AST node with variable name.
    /// </summary>
    public GraphQLVariable Variable { get; set; } = null!;

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with variable type.
    /// </summary>
    public GraphQLType Type { get; set; } = null!;

    /// <inheritdoc />
    public GraphQLValue? DefaultValue { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLVariableDefinitionWithLocation : GraphQLVariableDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLVariableDefinitionWithComment : GraphQLVariableDefinition
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}
internal sealed class GraphQLVariableDefinitionFull : GraphQLVariableDefinition
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
