using System.Collections.Generic;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ObjectValue"/>.
/// </summary>
public class GraphQLObjectValue : GraphQLValue
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ObjectValue;

    /// <summary>
    /// Values of the object represented as a list of nested <see cref="GraphQLObjectField"/> nodes.
    /// </summary>
    public List<GraphQLObjectField>? Fields { get; set; }

    /// <summary>
    /// Returns the first matching field node contained within this object value
    /// node that matches the specified name, or <see langword="null"/> otherwise.
    /// </summary>
    public GraphQLObjectField? Field(ROM name)
    {
        // DO NOT USE LINQ ON HOT PATH
        if (Fields != null)
        {
            foreach (var field in Fields)
            {
                if (field.Name.Value == name)
                    return field;
            }
        }

        return null;
    }
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
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLObjectValueFull : GraphQLObjectValue
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
