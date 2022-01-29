using System.Collections.Generic;
using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.VariableDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLVariableDefinition: {Variable}")]
public class GraphQLVariableDefinition : ASTNode, IHasDirectivesNode, IHasDefaultValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.VariableDefinition;

    /// <summary>
    /// Nested <see cref="GraphQLVariable"/> AST node with variable name.
    /// </summary>
    public GraphQLVariable Variable { get; set; } = null!;

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with variable type.
    /// </summary>
    public GraphQLType Type { get; set; } = null!;

    /// <inheritdoc />
    public GraphQLValue? DefaultValue { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }
}

internal sealed class GraphQLVariableDefinitionWithLocation : GraphQLVariableDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLVariableDefinitionWithComment : GraphQLVariableDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}
internal sealed class GraphQLVariableDefinitionFull : GraphQLVariableDefinition
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
