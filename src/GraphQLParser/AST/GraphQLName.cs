using System;
using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Name"/>.
/// </summary>
[DebuggerDisplay("GraphQLName: {Value}")]
public class GraphQLName : ASTNode, IHasValueNode, IEquatable<GraphQLName>
{
    private string? _string;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Name;

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
    public ROM Value { get; }

    /// <summary>
    /// Name value represented as <see cref="string"/>.
    /// <br/>
    /// This property allocates the string on the heap on first access
    /// and then caches it as long as <see cref="Value"/> does not change.
    /// </summary>
    public string StringValue
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
    public override string ToString() => StringValue;

    /// <summary>
    /// Implicitly casts <see cref="GraphQLName"/> to <see cref="ROM"/>.
    /// </summary>
    public static implicit operator ROM(GraphQLName? node) => node == null ? default : node.Value;

    /// <summary>
    /// Explicitly casts <see cref="GraphQLName"/> to <see cref="string"/>.
    /// </summary>
    public static explicit operator string(GraphQLName? node) => node == null ? null! : (string)node.Value; //TODO: not sure about nullability annotations for operators

    /// <summary>
    /// Checks two names for equality. The check is based on the actual contents of the two chunks of memory.
    /// </summary>
    public static bool operator ==(GraphQLName? name1, GraphQLName? name2) => Equals(name1, name2);

    /// <summary>
    /// Checks two names for inequality. The check is based on the actual contents of the two chunks of memory.
    /// </summary>
    public static bool operator !=(GraphQLName? name1, GraphQLName? name2) => !Equals(name1, name2);

    /// <inheritdoc/>
    public bool Equals(GraphQLName other) => Equals(this, other);

    private static bool Equals(GraphQLName? name1, GraphQLName? name2)
    {
        if (name1 is null)
            return name2 is null || name2.Value.IsEmpty;

        if (name2 is null)
            return name1.Value.IsEmpty;

        return name1.Value == name2.Value;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj) => obj is GraphQLName name && Equals(name);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();
}

internal sealed class GraphQLNameWithLocation : GraphQLName
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    /// <inheritdoc cref="GraphQLName(ROM)"/>
    public GraphQLNameWithLocation(ROM value)
        : base(value)
    {
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

    /// <inheritdoc cref="GraphQLName(ROM)"/>
    public GraphQLNameWithComment(ROM value)
        : base(value)
    {
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

    /// <inheritdoc cref="GraphQLName(ROM)"/>
    public GraphQLNameFull(ROM value)
        : base(value)
    {
    }
}
