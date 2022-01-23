using System.Collections.Generic;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.UnionTypeDefinition"/>.
/// </summary>
public class GraphQLUnionTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
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
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLUnionTypeDefinitionWithComment : GraphQLUnionTypeDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLUnionTypeDefinitionFull : GraphQLUnionTypeDefinition
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
