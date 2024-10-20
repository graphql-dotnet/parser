using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.NullValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLNullValue: {Value}")]
public class GraphQLNullValue : GraphQLValue, IHasValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.NullValue;

    /// <summary>
    /// Null value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value => "null";
}

internal sealed class GraphQLNullValueWithLocation : GraphQLNullValue
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLNullValueWithComment : GraphQLNullValue
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLNullValueFull : GraphQLNullValue
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
