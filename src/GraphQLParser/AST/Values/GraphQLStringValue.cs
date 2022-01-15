using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.StringValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLStringValue: {Value}")]
public class GraphQLStringValue : GraphQLValue
{
    private string? _string;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.StringValue;

    /// <summary>
    /// Creates a new instance (with empty value).
    /// </summary>
    public GraphQLStringValue()
    {
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLStringValue(string value)
    {
        Value = value;
        _string = value;
    }

    /// <summary>
    /// String value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; set; }

    /// <summary>
    /// String value represented as <see cref="string"/>.
    /// <br/>
    /// This property allocates the string on the heap on first access
    /// and then caches it. Call <see cref="Reset"/> to reset cache when needed.
    /// </summary>
    public string TypedValue
    {
        get
        {
            if (_string == null)
            {
                _string = Value.Length == 0
                    ? string.Empty
                    : (string)Value;
            }

            return _string;
        }
    }

    /// <inheritdoc />
    public override object? ClrValue => _string ??= TypedValue;

    /// <inheritdoc />
    public override void Reset()
    {
        _string = null;
    }
}

internal sealed class GraphQLStringValueWithLocation : GraphQLStringValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLStringValueWithComment : GraphQLStringValue
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLStringValueFull : GraphQLStringValue
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
