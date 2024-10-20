using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.UnionTypeDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLUnionTypeDefinition: {Name}")]
public class GraphQLUnionTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    internal GraphQLUnionTypeDefinition()
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLUnionTypeDefinition"/>.
    /// </summary>
    public GraphQLUnionTypeDefinition(GraphQLName name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.UnionTypeDefinition;

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLUnionMemberTypes"/> AST node with types contained in this union AST node.
    /// </summary>
    public GraphQLUnionMemberTypes? Types { get; set; }
}

internal sealed class GraphQLUnionTypeDefinitionWithLocation : GraphQLUnionTypeDefinition
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLUnionTypeDefinitionWithComment : GraphQLUnionTypeDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLUnionTypeDefinitionFull : GraphQLUnionTypeDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
