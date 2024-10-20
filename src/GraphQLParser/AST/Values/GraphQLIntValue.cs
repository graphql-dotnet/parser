using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.IntValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLIntValue: {Value}")]
public class GraphQLIntValue : GraphQLValue, IHasValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.IntValue;

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(ROM value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(int value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(long value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(ulong value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(BigInteger value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Integer value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; }
}

internal sealed class GraphQLIntValueWithLocation : GraphQLIntValue
{
    public override GraphQLLocation Location { get; set; }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLIntValueWithLocation(ROM value)
        : base(value)
    {
    }
}

internal sealed class GraphQLIntValueWithComment : GraphQLIntValue
{
    public override List<GraphQLComment>? Comments { get; set; }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLIntValueWithComment(ROM value)
        : base(value)
    {
    }
}

internal sealed class GraphQLIntValueFull : GraphQLIntValue
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLIntValueFull(ROM value)
        : base(value)
    {
    }
}
