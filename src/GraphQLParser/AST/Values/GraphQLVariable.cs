using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Variable"/>.
/// </summary>
[DebuggerDisplay("GraphQLVariable: {Name}")]
public class GraphQLVariable : GraphQLValue, INamedNode
{
    internal GraphQLVariable()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLVariable"/>.
    /// </summary>
    public GraphQLVariable(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Variable;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }
}

internal sealed class GraphQLVariableWithLocation : GraphQLVariable
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLVariableWithComment : GraphQLVariable
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLVariableFull : GraphQLVariable
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
