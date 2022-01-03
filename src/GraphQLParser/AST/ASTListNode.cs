using System.Collections;
using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents a AST node that holds a list of other (nested) AST nodes.
    /// </summary>
    public abstract class ASTListNode<TNode> : ASTNode, IEnumerable<TNode>
    {
        /// <summary>
        /// A list of nested AST nodes.
        /// </summary>
        public List<TNode> Items { get; set; } = null!;

        /// <summary>
        /// Get the number of AST nodes in the list.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Gets nested AST node by its index in the list.
        /// </summary>
        public TNode this[int index] => Items[index];

        public IEnumerator<TNode> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
