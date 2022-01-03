using GraphQLParser.AST;
using GraphQLParser.Visitors;

namespace GraphQLParser
{
    /// <summary>
    /// Extension methods for <see cref="ASTNode"/>.
    /// </summary>
    public static class ASTNodeExtensions
    {
        /// <summary>
        /// Counts the number of all nested nodes of the specified node along with that node itself.
        /// </summary>
        /// <param name="node">Parent node.</param>
        /// <returns>The number of nodes.</returns>
        public static int Count(this ASTNode node)
        {
            var visitor = new CountVisitor<DefaultCountContext>();
            var context = new DefaultCountContext(_ => true);
            visitor.Visit(node, context).GetAwaiter().GetResult(); // it's safe since method is actually sync
            return context.Count;
        }

        /// <summary>
        /// Gets the maximum depth for this AST node.
        /// </summary>
        /// <param name="node">Parent node.</param>
        /// <returns>Maximum depth. Minimum depth is 1.</returns>
        public static int MaxDepth(this ASTNode node)
        {
            var visitor = new MaxDepthVisitor<DefaultMaxDepthContext>();
            var context = new DefaultMaxDepthContext();
            visitor.Visit(node, context).GetAwaiter().GetResult(); // it's safe since method is actually sync
            return context.MaxDepth;
        }
    }
}
