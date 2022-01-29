using System.Collections.Generic;
using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Directive"/>.
/// </summary>
[DebuggerDisplay("GraphQLDirective: {Name}")]
public class GraphQLDirective : ASTNode, INamedNode, IHasArgumentsNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Directive;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;

    /// <summary>
    /// Arguments for this directive.
    /// </summary>
    public GraphQLArguments? Arguments { get; set; }
}

internal sealed class GraphQLDirectiveWithLocation : GraphQLDirective
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLDirectiveWithComment : GraphQLDirective
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLDirectiveFull : GraphQLDirective
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
