using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.NullValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLNullValue: {Value}")]
public class GraphQLNullValue : GraphQLValue, IHasValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.NullValue;

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLNullValue"/>.
    /// </summary>
    public GraphQLNullValue()
    {
        Value = "null";
    }

    /// <summary>
    /// Null value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; set; }
}

internal sealed class GraphQLNullValueWithLocation : GraphQLNullValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLNullValueWithComment : GraphQLNullValue
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLNullValueFull : GraphQLNullValue
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
