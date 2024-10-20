using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ScalarTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLScalarTypeDefinition: {Name}")]
public class GraphQLScalarTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    internal GraphQLScalarTypeDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLScalarTypeDefinition"/>.
    /// </summary>
    public GraphQLScalarTypeDefinition(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ScalarTypeDefinition;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLScalarTypeDefinitionWithLocation : GraphQLScalarTypeDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLScalarTypeDefinitionWithComment : GraphQLScalarTypeDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLScalarTypeDefinitionFull : GraphQLScalarTypeDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
