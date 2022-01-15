using System;
using System.Diagnostics;
using System.Globalization;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FloatValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLFloatValue: {Value}")]
public class GraphQLFloatValue : GraphQLValue
{
    private ROM _value;
    private object? _number;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FloatValue;

    /// <summary>
    /// Creates a new instance (with empty value).
    /// </summary>
    public GraphQLFloatValue()
    {
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLFloatValue(float value)
    {
        _number = ValidateValue(value);
        // print most compact form of value with up to 15 digits of precision (C# default)
        // note: G17 format (17 digits of precision) is necessary to prevent losing any
        // information during roundtrip to string. However, "3.33" prints something like
        // "3.330000000000001" which probably is not desirable.
        _value = ((double)value).ToString("G15", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLFloatValue(double value)
    {
        _number = ValidateValue(value);
        // print most compact form of value with up to 15 digits of precision (C# default)
        // note: G17 format (17 digits of precision) is necessary to prevent losing any
        // information during roundtrip to string. However, "3.33" prints something like
        // "3.330000000000001" which probably is not desirable.
        _value = value.ToString("G15", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLFloatValue(decimal value)
    {
        _number = value;
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Float value represented as <see cref="ROM"/>.
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

    private static double ValidateValue(double value)
    {
        // TODO: see https://github.com/graphql-dotnet/graphql-dotnet/pull/2379#issuecomment-800828568 and https://github.com/graphql-dotnet/graphql-dotnet/pull/2379#issuecomment-800906086
        // if (double.IsNaN(value) || double.IsInfinity(value))
        if (double.IsNaN(value))
            throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be NaN."); // Value cannot be NaN or Infinity.

        return value;
    }

    /// <summary>
    /// Float value represented as <see cref="double"/> or <see cref="decimal"/>.
    /// <br/>
    /// This property allocates the string on the heap on first access
    /// and then caches it as long as <see cref="Value"/> does not change.
    /// </summary>
    private object TypedValue //TODO: ??? no typed value :(
    {
        get
        {
            if (Value.Length == 0)
                throw new InvalidOperationException("Invalid number (empty string)");

            // the idea is to see if there is a loss of accuracy of value
            // for example, 12.1 or 12.11 is double but 12.10 is decimal
            if (!Double.TryParse(
                Value,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent,
                CultureInfo.InvariantCulture,
                out double dbl))
            {
                dbl = Value.Span[0] == '-' ? double.NegativeInfinity : double.PositiveInfinity;
            }

            //it is possible for a FloatValue to overflow a decimal; however, with a double, it just returns Infinity or -Infinity
            if (Decimal.TryParse(
                Value,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent,
                CultureInfo.InvariantCulture,
                out decimal dec))
            {
                // Cast the decimal to our struct to avoid the decimal.GetBits allocations.
                var decBits = System.Runtime.CompilerServices.Unsafe.As<decimal, DecimalData>(ref dec);
                decimal temp = new decimal(dbl);
                var dblAsDecBits = System.Runtime.CompilerServices.Unsafe.As<decimal, DecimalData>(ref temp);
                if (!decBits.Equals(dblAsDecBits))
                    return dec;
            }

            return dbl;
        }
    }

    /// <inheritdoc />
    public override object? ClrValue => _number ??= TypedValue;
}

internal sealed class GraphQLFloatValueWithLocation : GraphQLFloatValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFloatValueWithComment : GraphQLFloatValue
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLFloatValueFull : GraphQLFloatValue
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
