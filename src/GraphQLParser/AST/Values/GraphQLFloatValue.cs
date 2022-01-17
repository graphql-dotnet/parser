using System;
using System.Diagnostics;
using System.Globalization;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FloatValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLFloatValue: {Value}")]
public class GraphQLFloatValue : GraphQLValue, IHasValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FloatValue;

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLFloatValue(ROM value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLFloatValue(float value)
    {
        // print most compact form of value with up to 7 digits of precision (C# default)
        // note: G9 format (9 digits of precision) is necessary to prevent losing any
        // information during roundtrip to string. However, "3.33" prints something like
        // "3.33000001" which probably is not desirable.
        Value = value.ToString("R", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLFloatValue(double value)
    {
        // print most compact form of value with up to 15 digits of precision (C# default)
        // note: G17 format (17 digits of precision) is necessary to prevent losing any
        // information during roundtrip to string. However, "3.33" prints something like
        // "3.330000000000001" which probably is not desirable.
        Value = ValidateValue(value).ToString("G15", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLFloatValue(decimal value)
    {
        Value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Float value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; }

    private static double ValidateValue(double value)
    {
        // TODO: see https://github.com/graphql-dotnet/graphql-dotnet/pull/2379#issuecomment-800828568 and https://github.com/graphql-dotnet/graphql-dotnet/pull/2379#issuecomment-800906086
        // if (double.IsNaN(value) || double.IsInfinity(value))
        if (double.IsNaN(value))
            throw new ArgumentOutOfRangeException(nameof(value), "Value cannot be NaN."); // Value cannot be NaN or Infinity.

        return value;
    }
}

internal sealed class GraphQLFloatValueWithLocation : GraphQLFloatValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLFloatValueWithLocation(ROM value)
        : base(value)
    {
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

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLFloatValueWithComment(ROM value)
        : base(value)
    {
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

    /// <inheritdoc cref="GraphQLFloatValue(ROM)"/>
    public GraphQLFloatValueFull(ROM value)
        : base(value)
    {
    }
}
