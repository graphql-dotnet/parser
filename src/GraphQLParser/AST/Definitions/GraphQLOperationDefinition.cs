using System.Collections.Generic;
using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.OperationDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLOperationDefinition: {Operation}")]
public class GraphQLOperationDefinition : GraphQLExecutableDefinition, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.OperationDefinition;

    /// <summary>
    /// Type of operation definition.
    /// </summary>
    public OperationType Operation { get; set; }

    /// <summary>
    /// Name of the operation represented as a nested node.
    /// <br/>
    /// Note that name may be <see langword="null"/> for anonymous query.
    /// Therefore, the compiler shows CS8766 warning about nullability mismatch.
    /// </summary>
#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public GraphQLName? Name { get; set; }
#pragma warning restore CS8766

    /// <summary>
    /// Nested <see cref="GraphQLVariablesDefinition"/> AST node with operation variables (if any).
    /// </summary>
    public GraphQLVariablesDefinition? Variables { get; set; }
}

internal sealed class GraphQLOperationDefinitionWithLocation : GraphQLOperationDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLOperationDefinitionWithComment : GraphQLOperationDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLOperationDefinitionFull : GraphQLOperationDefinition
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
