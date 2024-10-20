using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.DirectiveDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLDirectiveDefinition: {Name}")]
public class GraphQLDirectiveDefinition : GraphQLTypeDefinition, IHasArgumentsDefinitionNode
{
    internal GraphQLDirectiveDefinition()
    {
        Locations = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLDirectiveDefinition"/>.
    /// </summary>
    public GraphQLDirectiveDefinition(GraphQLName name, GraphQLDirectiveLocations locations)
        : base(name)
    {
        Locations = locations;
    }

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
    public GraphQLDirectiveLocations Locations { get; set; }
}

internal sealed class GraphQLDirectiveDefinitionWithLocation : GraphQLDirectiveDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLDirectiveDefinitionWithComment : GraphQLDirectiveDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLDirectiveDefinitionFull : GraphQLDirectiveDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
