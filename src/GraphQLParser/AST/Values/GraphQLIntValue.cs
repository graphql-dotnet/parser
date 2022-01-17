using System;
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
    /// Creates a new instance (with empty value).
    /// </summary>
    public GraphQLIntValue()
    {
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
    public GraphQLIntValue(byte value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(sbyte value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(short value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(ushort value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(uint value)
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
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(decimal value)
    {
        if (Math.Truncate(value) != value)
            throw new ArgumentOutOfRangeException(nameof(value), $"Invalid integer number {value}");
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Integer value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; set; }
}

internal sealed class GraphQLIntValueWithLocation : GraphQLIntValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
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
}
