using System.Collections.Generic;
using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ListValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLListValue: {Value}")]
public class GraphQLListValue : GraphQLValue
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ListValue;

    /// <summary>
    /// Values of the list represented as a list of nested <see cref="GraphQLValue"/> nodes.
    /// </summary>
    public List<GraphQLValue>? Values { get; set; }
}

internal sealed class GraphQLListValueWithLocation : GraphQLListValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLListValueWithComment : GraphQLListValue
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLListValueFull : GraphQLListValue
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
