using System.Collections.Generic;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Variable"/>.
/// </summary>
public class GraphQLVariable : GraphQLValue, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Variable;

    /// <inheritdoc/>
    public GraphQLName Name { get; set; } = null!;
}

internal sealed class GraphQLVariableWithLocation : GraphQLVariable
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLVariableWithComment : GraphQLVariable
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLVariableFull : GraphQLVariable
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
