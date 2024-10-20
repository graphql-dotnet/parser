using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Directive"/>.
/// </summary>
[DebuggerDisplay("GraphQLDirective: {Name}")]
public class GraphQLDirective : ASTNode, INamedNode, IHasArgumentsNode
{
    internal GraphQLDirective()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLDirective"/>.
    /// </summary>
    public GraphQLDirective(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Directive;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }

    /// <summary>
    /// Arguments for this directive.
    /// </summary>
    public GraphQLArguments? Arguments { get; set; }
}

internal sealed class GraphQLDirectiveWithLocation : GraphQLDirective
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLDirectiveWithComment : GraphQLDirective
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLDirectiveFull : GraphQLDirective
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
