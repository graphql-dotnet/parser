using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ListValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLListValue: {Value}")]
public class GraphQLListValue : GraphQLValue
{
    private ROM _value;
    private List<object?>? _list;

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ListValue;

    /// <summary>
    /// List value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value
    {
        get => _value;
        set
        {
            _value = value;
            _list = null;
        }
    }

    /// <summary>
    /// Values of the list represented as a list of nested <see cref="GraphQLValue"/> nodes.
    /// </summary>
    public List<GraphQLValue>? Values { get; set; }

    /// <summary>
    /// List value represented as <see cref="List{T}"/>.
    /// <br/>
    /// This property allocates the string on the heap on first access
    /// and then caches it as long as <see cref="Value"/> does not change.
    /// </summary>
    public IList<object?> TypedValue
    {
        get
        {
            if (Values == null || Values.Count == 0)
                return Array.Empty<object?>();

            if (_list == null)
            {
                var list = new List<object?>(Values.Count);
                foreach (var value in Values)
                    list.Add(value.ClrValue);
                _list = list;
            }

            return _list;
        }
    }

    /// <inheritdoc />
    public override object? ClrValue => TypedValue;
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
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLListValueFull : GraphQLListValue
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
