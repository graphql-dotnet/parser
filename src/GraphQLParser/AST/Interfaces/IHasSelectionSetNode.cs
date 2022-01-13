namespace GraphQLParser.AST;

/// <summary>
/// Represents an AST node that may have selection set.
/// </summary>
public interface IHasSelectionSetNode
{
    /// <summary>
    /// Nested <see cref="GraphQLSelectionSet"/> AST node with selection set of this AST node.
    /// </summary>
    GraphQLSelectionSet? SelectionSet { get; set; }
}
