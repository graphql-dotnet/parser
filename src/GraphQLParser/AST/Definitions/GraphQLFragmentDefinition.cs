using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FragmentDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLFragmentDefinition: {FragmentName.Name.StringValue}")]
public class GraphQLFragmentDefinition : GraphQLExecutableDefinition
{
    internal GraphQLFragmentDefinition()
    {
        FragmentName = null!;
        TypeCondition = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLFragmentDefinition"/>.
    /// </summary>
    public GraphQLFragmentDefinition(GraphQLFragmentName name, GraphQLTypeCondition typeCondition, GraphQLSelectionSet selectionSet)
        : base(selectionSet)
    {
        FragmentName = name;
        TypeCondition = typeCondition;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FragmentDefinition;

    /// <summary>
    /// Fragment name represented as a nested AST node.
    /// </summary>
    public GraphQLFragmentName FragmentName { get; set; }

    /// <summary>
    /// Nested <see cref="GraphQLTypeCondition"/> AST node with type condition of this fragment.
    /// </summary>
    public GraphQLTypeCondition TypeCondition { get; set; }
}

internal sealed class GraphQLFragmentDefinitionWithLocation : GraphQLFragmentDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFragmentDefinitionWithComment : GraphQLFragmentDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}
internal sealed class GraphQLFragmentDefinitionFull : GraphQLFragmentDefinition
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
