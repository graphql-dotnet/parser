using System.IO;
using System.Threading.Tasks;
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
        public override async ValueTask Visit(ASTNode? node, TContext context)
        {
            if (node == null)
                return;

            for (int i = 0; i < context.Parent.Count; ++i)
                await context.Write("  ");

            context.Parent.Push(node);
            await context.Write(node.Kind.ToString());
            await context.WriteLine();
            await base.Visit(node, context);
            context.Parent.Pop();
        }
    }
}
