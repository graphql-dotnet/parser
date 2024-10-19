using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumValueDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLEnumValueDefinition: {EnumValue}")]
public class GraphQLEnumValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    internal GraphQLEnumValueDefinition()
    {
        EnumValue = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLEnumValueDefinition"/>.
    /// </summary>
    public GraphQLEnumValueDefinition(GraphQLName name, GraphQLEnumValue enumValue)
        : base(name)
    {
        EnumValue = enumValue;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.EnumValueDefinition;

    /// <summary>
    /// Enum value represented as a nested AST node. Alas, inherited
    /// <see cref="GraphQLTypeDefinition.Name"/> property holds almost
    /// the same data and should be set as well.
    /// </summary>
    public GraphQLEnumValue EnumValue { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc />
    public override bool IsChildDefinition => true;
}

internal sealed class GraphQLEnumValueDefinitionWithLocation : GraphQLEnumValueDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLEnumValueDefinitionWithComment : GraphQLEnumValueDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLEnumValueDefinitionFull : GraphQLEnumValueDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
