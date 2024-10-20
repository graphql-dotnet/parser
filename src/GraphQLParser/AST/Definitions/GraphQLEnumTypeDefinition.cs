using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLEnumTypeDefinition: {Name}")]
public class GraphQLEnumTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    internal GraphQLEnumTypeDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLEnumTypeDefinition"/>.
    /// </summary>
    public GraphQLEnumTypeDefinition(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.EnumTypeDefinition;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLEnumValuesDefinition"/> AST node with enum values.
    /// </summary>
    public GraphQLEnumValuesDefinition? Values { get; set; }
}

internal sealed class GraphQLEnumTypeDefinitionWithLocation : GraphQLEnumTypeDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLEnumTypeDefinitionWithComment : GraphQLEnumTypeDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLEnumTypeDefinitionFull : GraphQLEnumTypeDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
