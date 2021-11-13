using System.IO;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors
{
    /// <summary>
    /// Prints AST into the provided <see cref="TextWriter"/> as a hierarchy of node types.
    /// </summary>
    /// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
    public class StructureWriter<TContext> : DefaultNodeVisitor<TContext>
        where TContext : IWriteContext
    {
        /// <inheritdoc/>
        public override void Visit(ASTNode? node, TContext context)
        {
            if (node == null)
                return;

            for (int i = 0; i < context.Parent.Count; ++i)
                context.Writer.Write("  ");

            context.Parent.Push(node);
            context.Writer.WriteLine(node.Kind.ToString());
            base.Visit(node, context);
            context.Parent.Pop();
        }
    }
}
