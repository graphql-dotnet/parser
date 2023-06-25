using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLEnumValue: {Name}")]
public class GraphQLEnumValue : GraphQLValue, INamedNode
{
    /// <summary>
    /// Creates a new instance of <see cref="GraphQLEnumValue"/>.
    /// </summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLEnumValue()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLEnumValue"/>.
    /// </summary>
    public GraphQLEnumValue(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.EnumValue;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;
}

internal sealed class GraphQLEnumValueWithLocation : GraphQLEnumValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLEnumValueWithComment : GraphQLEnumValue
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLEnumValueFull : GraphQLEnumValue
{
    private GraphQLLocation _location;
    private List<GraphQLComment>? _comments;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}
