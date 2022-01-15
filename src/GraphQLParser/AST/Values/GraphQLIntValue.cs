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
    private ROM _value;
    private object? _number;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.IntValue;

    /// <summary>
    /// Integer value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value
    {
        get => _value;
        set
        {
            _value = value;
            _number = null;
        }
    }

    /// <summary>
    /// Integer value represented as <see cref="int"/>, <see cref="long"/>, <see cref="decimal"/> or <see cref="BigInteger"/>.
    /// <br/>
    /// This property allocates the string on the heap on first access
    /// and then caches it as long as <see cref="Value"/> does not change.
    /// </summary>
    private /*public*/ object TypedValue //TODO: ??? no typed value :(
    {
        get
        {
            return _value.Length == 0
                ? throw new InvalidOperationException("Invalid number (empty string)")
                : _number ??= Parse();

            object Parse()
            {
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
    }

    /// <inheritdoc />
    public override object? ClrValue => TypedValue;
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
