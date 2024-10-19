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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLFragmentDefinitionWithComment : GraphQLFragmentDefinition
{
    public override List<GraphQLComment>? Comments { get; set; }
}
internal sealed class GraphQLFragmentDefinitionFull : GraphQLFragmentDefinition
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
