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
