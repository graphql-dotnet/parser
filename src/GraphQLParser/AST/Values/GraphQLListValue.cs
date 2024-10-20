using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ListValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLListValue: {Value}")]
public class GraphQLListValue : GraphQLValue
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ListValue;

    /// <summary>
    /// Values of the list represented as a list of nested <see cref="GraphQLValue"/> nodes.
    /// </summary>
    public List<GraphQLValue>? Values { get; set; }
}

internal sealed class GraphQLListValueWithLocation : GraphQLListValue
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLListValueWithComment : GraphQLListValue
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLListValueFull : GraphQLListValue
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
