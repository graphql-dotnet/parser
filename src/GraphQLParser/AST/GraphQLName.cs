using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Name"/>.
/// </summary>
[DebuggerDisplay("GraphQLName: {Value}")]
public class GraphQLName : ASTNode, IHasValueNode, IEquatable<GraphQLName>
{
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
            field ??= Value.Length == 0
                    ? string.Empty
                    : (string)Value;

            return field;
        }

        private set;
    }

    /// <inheritdoc />
    public override string ToString() => StringValue;

    /// <summary>
    /// Implicitly casts <see cref="GraphQLName"/> to <see cref="ROM"/>.
    /// </summary>
    public static implicit operator ROM(GraphQLName? node) => node is null ? default : node.Value;

    /// <summary>
    /// Explicitly casts <see cref="GraphQLName"/> to <see cref="string"/>.
    /// </summary>
    public static explicit operator string(GraphQLName? node) => node is null ? null! : (string)node.Value; //TODO: not sure about nullability annotation for returned string here

    /// <summary>
    /// Checks two names for equality. The check is based on the actual contents of the two chunks of memory.
    /// </summary>
    public static bool operator ==(GraphQLName? name1, GraphQLName? name2) => Equals(name1, name2);

    /// <summary>
    /// Checks two names for inequality. The check is based on the actual contents of the two chunks of memory.
    /// </summary>
    public static bool operator !=(GraphQLName? name1, GraphQLName? name2) => !Equals(name1, name2);

    /// <summary>
    /// Checks GraphQLName and string for equality. The check is based on the actual contents of the two chunks of memory.
    /// </summary>
    public static bool operator ==(GraphQLName? name1, string? name2) => Equals(name1, name2);

    /// <summary>
    /// Checks GraphQLName and string for inequality. The check is based on the actual contents of the two chunks of memory.
    /// </summary>
    public static bool operator !=(GraphQLName? name1, string? name2) => !Equals(name1, name2);

    /// <summary>
    /// Checks string and GraphQLName for equality. The check is based on the actual contents of the two chunks of memory.
    /// </summary>
    public static bool operator ==(string? name1, GraphQLName? name2) => name2 == name1;

    /// <summary>
    /// Checks string and GraphQLName for inequality. The check is based on the actual contents of the two chunks of memory.
    /// </summary>
    public static bool operator !=(string? name1, GraphQLName? name2) => name2 != name1;

    /// <inheritdoc/>
    public bool Equals(GraphQLName? other) => Equals(this, other);

    private static bool Equals(GraphQLName? name1, GraphQLName? name2)
    {
        if (name1 is null)
            return name2 is null || name2.Value.IsEmpty;

        if (name2 is null)
            return name1.Value.IsEmpty;

        return name1.Value == name2.Value;
    }

    private static bool Equals(GraphQLName? name1, string? name2)
    {
        if (name1 is null)
            return string.IsNullOrEmpty(name2);

        if (name2 is null)
            return name1.Value.IsEmpty;

        return name1.Value == name2;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is GraphQLName name && Equals(name);

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();
}

internal sealed class GraphQLNameWithLocation : GraphQLName
{
    public override GraphQLLocation Location { get; set; }

    /// <inheritdoc cref="GraphQLName(ROM)"/>
    public GraphQLNameWithLocation(ROM value)
        : base(value)
    {
    }
}

internal sealed class GraphQLNameWithComment : GraphQLName
{
    public override List<GraphQLComment>? Comments { get; set; }

    /// <inheritdoc cref="GraphQLName(ROM)"/>
    public GraphQLNameWithComment(ROM value)
        : base(value)
    {
    }
}

internal sealed class GraphQLNameFull : GraphQLName
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }

    /// <inheritdoc cref="GraphQLName(ROM)"/>
    public GraphQLNameFull(ROM value)
        : base(value)
    {
    }
}
