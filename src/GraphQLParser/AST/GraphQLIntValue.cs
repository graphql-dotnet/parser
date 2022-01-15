using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.IntValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLIntValue: {Value}")]
public class GraphQLIntValue : GraphQLValue
{
    private object? _number;

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
        _number = value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(byte value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        _number = (int)value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(sbyte value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        _number = (int)value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(short value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        _number = (int)value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(ushort value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        _number = (int)value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(uint value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        if (value < int.MaxValue)
            _number = (int)value;
        else
            _number = (long)value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(long value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        _number = value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(ulong value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        if (value < int.MaxValue)
            _number = (int)value;
        else if (value < long.MaxValue)
            _number = (long)value;
        else
            _number = (decimal)value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(BigInteger value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        _number = value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLIntValue(decimal value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
        _number = value;
    }

    /// <summary>
    /// Integer value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; set; }

    /// <summary>
    /// Integer value represented as <see cref="int"/>, <see cref="long"/>, <see cref="decimal"/> or <see cref="BigInteger"/>.
    /// <br/>
    /// This property allocates the string on the heap on first access
    /// and then caches it. Call <see cref="Reset"/> to reset cache when needed.
    /// </summary>
    private object TypedValue //TODO: ??? no typed value :(
    {
        get
        {
            if (Value.Length == 0)
                throw new InvalidOperationException("Invalid number (empty string)");

            if (Int.TryParse(Value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int intResult))
            {
                return intResult;
            }

            // If the value doesn't fit in an integer, revert to using long...
            if (Long.TryParse(Value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out long longResult))
            {
                return longResult;
            }

            // If the value doesn't fit in an long, revert to using decimal...
            if (Decimal.TryParse(Value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out decimal decimalResult))
            {
                return decimalResult;
            }

            // If the value doesn't fit in an decimal, revert to using BigInteger...
            if (BigInt.TryParse(Value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var bigIntegerResult))
            {
                return bigIntegerResult;
            }

            // Since BigInteger can contain any valid integer (arbitrarily large), this is impossible to trigger via an invalid query
            throw new InvalidOperationException($"Invalid number {Value}");
        }
    }

    /// <inheritdoc />
    public override object? ClrValue => _number ??= TypedValue;

    /// <inheritdoc />
    public override void Reset()
    {
        _number = null;
    }
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
