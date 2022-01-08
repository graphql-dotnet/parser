using System;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Counts AST nodes.
/// </summary>
/// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
public class CountVisitor<TContext> : DefaultNodeVisitor<TContext>
    where TContext : ICountContext
{
    /// <inheritdoc/>
    public override ValueTask Visit(ASTNode? node, TContext context)
    {
        if (node != null && context.ShouldInclude(node))
            ++context.Count;

        return base.Visit(node, context);
    }
}

/// <summary>
/// Context used by <see cref="CountVisitor{TContext}"/>.
/// </summary>
public interface ICountContext : INodeVisitorContext
{
    /// <summary>
    /// Number of found AST nodes.
    /// </summary>
    int Count { get; set; }

    /// <summary>
    /// Predicate used to increment <see cref="Count"/>.
    /// </summary>
    Func<ASTNode, bool> ShouldInclude { get; }
}

/// <summary>
/// Default implementation for <see cref="ICountContext"/>.
/// </summary>
public class DefaultCountContext : ICountContext
{
    /// <summary>
    /// Creates context with the specified delegate.
    /// </summary>
    /// <param name="shouldInclude">Predicate used to increment <see cref="Count"/>.</param>
    public DefaultCountContext(Func<ASTNode, bool> shouldInclude)
    {
        ShouldInclude = shouldInclude;
    }

    /// <inheritdoc/>
    public CancellationToken CancellationToken { get; set; }

    /// <inheritdoc/>
    public int Count { get; set; }

    /// <inheritdoc/>
    public Func<ASTNode, bool> ShouldInclude { get; }
}
