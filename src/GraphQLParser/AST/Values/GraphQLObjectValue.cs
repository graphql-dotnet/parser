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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLObjectValueWithComment : GraphQLObjectValue
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLObjectValueFull : GraphQLObjectValue
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
