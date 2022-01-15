using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.BooleanValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLBooleanValue: {Value}")]
public class GraphQLBooleanValue : GraphQLValue
{
    private static readonly object _true = true;
    private static readonly object _false = false;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.BooleanValue;

    /// <summary>
    /// Creates a new instance (with empty value).
    /// </summary>
    public GraphQLBooleanValue()
    {
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLBooleanValue(bool value)
    {
        Value = value ? "true" : "false";
    }

    /// <summary>
    /// Boolean value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; set; }

    /// <inheritdoc />
    public override object? ClrValue => TypedValue ? _true : _false;

    /// <inheritdoc cref="GraphQLValue.ClrValue"/>
    public bool TypedValue => Value == "true" ? true : false;
}

internal sealed class GraphQLBooleanValueWithLocation : GraphQLBooleanValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLBooleanValueWithComment : GraphQLBooleanValue
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLBooleanValueFull : GraphQLBooleanValue
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
