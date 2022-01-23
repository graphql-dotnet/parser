using System.Collections.Generic;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Argument"/>.
/// </summary>
public class GraphQLArgument : ASTNode, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Argument;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;

    /// <summary>
    /// Nested <see cref="GraphQLValue"/> AST node with argument value.
    /// </summary>
    public GraphQLValue Value { get; set; } = null!;
}

internal sealed class GraphQLArgumentWithLocation : GraphQLArgument
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLArgumentWithComment : GraphQLArgument
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLArgumentFull : GraphQLArgument
{
    private GraphQLLocation _location;
    private List<GraphQLComment>? _comments;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}
