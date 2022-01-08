namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.FragmentDefinition"/>.
/// </summary>
public class GraphQLFragmentDefinition : GraphQLExecutableDefinition, INamedNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.FragmentDefinition;

    /// <summary>
    /// Name of the fragment represented as a nested AST node.
    /// http://spec.graphql.org/October2021/#FragmentName
    /// </summary>
    public GraphQLName Name { get; set; } = null!;

    /// <summary>
    /// Nested <see cref="GraphQLTypeCondition"/> AST node with type condition of this fragment.
    /// </summary>
    public GraphQLTypeCondition TypeCondition { get; set; } = null!;
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
    private GraphQLComment? _comment;

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}
internal sealed class GraphQLFragmentDefinitionFull : GraphQLFragmentDefinition
{
    private GraphQLLocation _location;
    private GraphQLComment? _comment;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override GraphQLComment? Comment
    {
        get => _comment;
        set => _comment = value;
    }
}
