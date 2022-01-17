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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLIntValueWithLocation(ROM value)
        : base(value)
    {
    }
}

internal sealed class GraphQLIntValueWithComment : GraphQLIntValue
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLIntValueWithComment(ROM value)
        : base(value)
    {
    }
}

internal sealed class GraphQLIntValueFull : GraphQLIntValue
{
    private GraphQLLocation _location;
    private GraphQLComment? _comment;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLIntValueFull(ROM value)
        : base(value)
    {
    }
}
