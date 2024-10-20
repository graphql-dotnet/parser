using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.StringValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLStringValue: {Value}")]
public class GraphQLStringValue : GraphQLValue, IHasValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.StringValue;

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLStringValue(ROM value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLStringValue(string value)
    {
        Value = value;
    }

    /// <summary>
    /// String value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; }
}

internal sealed class GraphQLStringValueWithLocation : GraphQLStringValue
{
    public override GraphQLLocation Location { get; set; }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLStringValueWithLocation(ROM value)
        : base(value)
    {
    }
}

internal sealed class GraphQLStringValueWithComment : GraphQLStringValue
{
    public override List<GraphQLComment>? Comments { get; set; }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLStringValueWithComment(ROM value)
        : base(value)
    {
    }
}

internal sealed class GraphQLStringValueFull : GraphQLStringValue
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLStringValueFull(ROM value)
        : base(value)
    {
    }
}
