using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLEnumValue: {Name}")]
public class GraphQLEnumValue : GraphQLValue, INamedNode
{
    internal GraphQLEnumValue()
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
    public GraphQLName Name { get; set; }
}

internal sealed class GraphQLEnumValueWithLocation : GraphQLEnumValue
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLEnumValueWithComment : GraphQLEnumValue
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLEnumValueFull : GraphQLEnumValue
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
