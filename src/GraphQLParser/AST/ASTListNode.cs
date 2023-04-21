using System.Collections;

namespace GraphQLParser.AST;

/// <summary>
/// Represents a AST node that holds a list of other (nested) AST nodes.
/// </summary>
public abstract class ASTListNode<TNode> : ASTNode, IReadOnlyList<TNode>
{
    /// <summary>
    /// Creates a new instance of <see cref="ASTListNode{TNode}"/>.
    /// </summary>
    protected ASTListNode()
    {
        Items = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ASTListNode{TNode}"/>.
    /// </summary>
    protected ASTListNode(List<TNode> items)
    {
        Items = items;
    }

    /// <summary>
    /// A list of nested AST nodes.
    /// </summary>
    public List<TNode> Items { get; set; }

    /// <summary>
    /// Get the number of AST nodes in the list.
    /// </summary>
    public int Count => Items.Count;

    /// <summary>
    /// Gets nested AST node by its index in the list.
    /// </summary>
    public TNode this[int index] => Items[index];

    /// <inheritdoc />
    public IEnumerator<TNode> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
