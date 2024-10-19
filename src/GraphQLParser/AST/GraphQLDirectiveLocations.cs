namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.DirectiveLocations"/>.
/// </summary>
public class GraphQLDirectiveLocations : ASTNode // no ASTListNode<DirectiveLocation> since DirectiveLocation is enum
{
    internal GraphQLDirectiveLocations()
    {
        Items = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLDirectiveLocations"/>.
    /// </summary>
    public GraphQLDirectiveLocations(List<DirectiveLocation> items)
    {
        Items = items;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.DirectiveLocations;

    /// <summary>
    /// A list of locations representing the valid locations where directive may be placed.
    /// </summary>
    public List<DirectiveLocation> Items { get; set; }
}

internal sealed class GraphQLDirectiveLocationsWithLocation : GraphQLDirectiveLocations
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLDirectiveLocationsWithComment : GraphQLDirectiveLocations
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLDirectiveLocationsFull : GraphQLDirectiveLocations
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
