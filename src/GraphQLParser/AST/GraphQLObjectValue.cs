using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectValue"/>.
/// </summary>
public class GraphQLObjectValue : GraphQLValue
{
    private ROM _value;
    private IDictionary<string, object?>? _object;
    private static readonly ReadOnlyDictionary<string, object?> _empty = new(new Dictionary<string, object?>());

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ObjectValue;

    /// <summary>
    /// Object value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value
    {
        get => _value;
        set
        {
            _value = value;
            _object = null;
        }
    }

    /// <summary>
    /// Values of the object represented as a list of nested <see cref="GraphQLObjectField"/> nodes.
    /// </summary>
    public List<GraphQLObjectField>? Fields { get; set; }

    /// <summary>
    /// Object value represented as <see cref="IDictionary{TKey, TValue}"/>.
    /// <br/>
    /// This property allocates the string on the heap on first access
    /// and then caches it as long as <see cref="Value"/> does not change.
    /// </summary>
    public IDictionary<string, object?> TypedValue
    {
        get
        {
            if (Value.Length == 0)
                throw new InvalidOperationException("Invalid object (empty string)");

            if (_object == null)
            {
                if (Fields == null || Fields.Count == 0)
                {
                    _object = _empty;
                }
                else
                {
                    var @object = new Dictionary<string, object?>(Fields.Count);
                    foreach (var field in Fields)
                        @object.Add(field.Name.StringValue, field.Value.ClrValue);
                    _object = @object;
                }
            }

            return _object;
        }
    }

    /// <inheritdoc />
    public override object? ClrValue => _object ??= TypedValue;
}

internal sealed class GraphQLObjectValueWithLocation : GraphQLObjectValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLObjectValueWithComment : GraphQLObjectValue
{
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}

internal sealed class GraphQLObjectValueFull : GraphQLObjectValue
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
