using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.BooleanValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLBooleanValue: {Value}")]
public class GraphQLBooleanValue : GraphQLValue, IHasValueNode
{
    private readonly bool _value;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.BooleanValue;

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLBooleanValue(bool value)
    {
        _value = value;
    }

    /// <summary>
    /// Boolean value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value => _value ? "true" : "false";
}

internal sealed class GraphQLBooleanValueWithLocation : GraphQLBooleanValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    /// <inheritdoc cref="GraphQLBooleanValue(bool)"/>
    public GraphQLBooleanValueWithLocation(bool value)
        : base(value)
    {
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

    /// <inheritdoc cref="GraphQLBooleanValue(bool)"/>
    public GraphQLBooleanValueWithComment(bool value)
        : base(value)
    {
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

    /// <inheritdoc cref="GraphQLBooleanValue(bool)"/>
    public GraphQLBooleanValueFull(bool value)
        : base(value)
    {
    }
}
