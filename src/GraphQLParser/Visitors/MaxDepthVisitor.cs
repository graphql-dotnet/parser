using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Gets the maximum depth for the specified AST node.
/// Minimum depth is 1.
/// </summary>
/// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
public class MaxDepthVisitor<TContext> : ASTVisitor<TContext>
    where TContext : IMaxDepthContext
{
    /// <inheritdoc/>
    public override async ValueTask VisitAsync(ASTNode? node, TContext context)
    {
        if (node != null)
        {
            context.Parents.Push(node);

            if (context.Parents.Count > context.MaxDepth)
                context.MaxDepth = context.Parents.Count;

            await base.VisitAsync(node, context).ConfigureAwait(false);

            context.Parents.Pop();
        }
    }
}

/// <summary>
/// Context used by <see cref="MaxDepthVisitor{TContext}"/>.
/// </summary>
public interface IMaxDepthContext : IASTVisitorContext
{
    /// <summary>
    /// Maximum depth of AST found. Minimum depth is 1.
    /// </summary>
    int MaxDepth { get; set; }

    /// <summary>
    /// Stack of AST nodes to track the current visitor position.
    /// </summary>
    Stack<ASTNode> Parents { get; }
}

/// <summary>
/// Default implementation for <see cref="IMaxDepthContext"/>.
/// </summary>
public class DefaultMaxDepthContext : IMaxDepthContext
{
    /// <inheritdoc/>
    public CancellationToken CancellationToken { get; init; }

    /// <inheritdoc/>
    public int MaxDepth { get; set; }

    /// <inheritdoc/>
    public Stack<ASTNode> Parents { get; init; } = new Stack<ASTNode>();
}
