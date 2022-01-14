using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Name"/>.
/// </summary>
[DebuggerDisplay("GraphQLName: {Value}")]
public class GraphQLName : ASTNode
{
    private ROM _value;
    private string? _string;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Name;

    /// <summary>
    /// Creates a new instance (with empty value).
    /// </summary>
    public GraphQLName()
    {
    }

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLName(ROM value)
    {
        Value = value;
    }


    /// <summary>
    /// Name value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value
    {
        get => _value;
        set
        {
            _value = value;
            _string = null;
        }
    }

    /// <summary>
    /// Name value represented as <see cref="string"/>.
    /// <br/>
    /// This property allocates the string on the heap on first access
    /// and then caches it until <see cref="Value"/> does not change.
    /// </summary>
    public string StringValue
    {
        get
        {
            if (_value.Length == 0)
                return string.Empty;

            if (_string == null)
                _string = (string)_value;

            return _string;
        }
    }

    /// <inheritdoc />
    public override string ToString() => StringValue;

    /// <summary>
    /// Implicitly casts <see cref="GraphQLName"/> to <see cref="ROM"/>.
    /// </summary>
    public static implicit operator ROM(GraphQLName? node) => node == null ? default : node.Value;

    /// <summary>
    /// Explicitly casts <see cref="GraphQLName"/> to <see cref="string"/>.
    /// </summary>
    public static explicit operator string(GraphQLName? node) => node == null ? null! : (string)node.Value; //TODO: not sure about nullability annotations for operators
}

internal sealed class GraphQLNameWithLocation : GraphQLName
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLNameWithComment : GraphQLName
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLNameFull : GraphQLName
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
