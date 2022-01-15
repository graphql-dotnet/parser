namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.DirectiveDefinition"/>.
/// </summary>
public class GraphQLDirectiveDefinition : GraphQLTypeDefinition, IHasArgumentsDefinitionNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.DirectiveDefinition;

    /// <summary>
    /// Arguments for this directive definition.
    /// </summary>
    public GraphQLArgumentsDefinition? Arguments { get; set; }

    /// <summary>
    /// Indicates if the directive may be used repeatedly at a single location.
    /// <br/><br/>
    /// Repeatable directives are often useful when the same directive
    /// should be used with different arguments at a single location,
    /// especially in cases where additional information needs to be
    /// provided to a type or schema extension via a directive
    /// </summary>
    public bool Repeatable { get; set; }

    /// <summary>
    /// Returns a node with a list of locations representing the valid locations this directive may be placed.
    /// </summary>
    public GraphQLDirectiveLocations Locations { get; set; } = null!;
}

internal sealed class GraphQLDirectiveDefinitionWithLocation : GraphQLDirectiveDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLDirectiveDefinitionWithComment : GraphQLDirectiveDefinition
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLDirectiveDefinitionFull : GraphQLDirectiveDefinition
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
