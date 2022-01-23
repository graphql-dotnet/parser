using System.Collections.Generic;
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
    /// Null value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value => "null";
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
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLNullValueFull : GraphQLNullValue
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
