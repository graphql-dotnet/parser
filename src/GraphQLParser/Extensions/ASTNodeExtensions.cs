using GraphQLParser.AST;
using GraphQLParser.Visitors;

namespace GraphQLParser;

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
    public static int AllNestedCount(this ASTNode node)
    {
        var visitor = new CountVisitor<DefaultCountContext>();
        var context = new DefaultCountContext(_ => true);
        visitor.VisitAsync(node, context).GetAwaiter().GetResult(); // it's safe since method is actually sync
        return context.Count;
    }

    /// <summary>
    /// Gets the maximum depth of all nested nodes' chains for this AST node.
    /// </summary>
    /// <param name="node">Parent node.</param>
    /// <returns>Maximum depth. Minimum depth is 1.</returns>
    public static int MaxNestedDepth(this ASTNode node)
    {
        var visitor = new MaxDepthVisitor<DefaultMaxDepthContext>();
        var context = new DefaultMaxDepthContext();
        visitor.VisitAsync(node, context).GetAwaiter().GetResult(); // it's safe since method is actually sync
        return context.MaxDepth;
    }

    /// <summary>
    /// Searches GraphQL document for the first matching operation with the
    /// specified name, or returns the first operation if name not specified.
    /// Returns <see langword="null"/> if none is found.
    /// </summary>
    public static GraphQLOperationDefinition? OperationWithName(this GraphQLDocument document, ROM operationName)
    {
        if (operationName.IsEmpty)
        {
            foreach (var def in document.Definitions)
            {
                if (def is GraphQLOperationDefinition op)
                    return op;
            }
        }
        else
        {
            foreach (var def in document.Definitions)
            {
                if (def is GraphQLOperationDefinition op && op.Name == operationName)
                    return op;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets count of operations in the specified GraphQL document.
    /// </summary>
    public static int OperationsCount(this GraphQLDocument document)
    {
        int count = 0;

        foreach (var def in document.Definitions)
        {
            if (def is GraphQLOperationDefinition)
                ++count;
        }

        return count;
    }

    /// <summary>
    /// Gets count of fragments in the specified GraphQL document.
    /// </summary>
    public static int FragmentsCount(this GraphQLDocument document)
    {
        int count = 0;

        foreach (var def in document.Definitions)
        {
            if (def is GraphQLFragmentDefinition)
                ++count;
        }

        return count;
    }

    /// <summary>
    /// Searches GraphQL document for the first matching fragment definition,
    /// or returns <see langword="null"/> if none is found.
    /// </summary>
    public static GraphQLFragmentDefinition? FindFragmentDefinition(this GraphQLDocument document, ROM name)
    {
        // DO NOT USE LINQ ON HOT PATH
        foreach (var def in document.Definitions)
        {
            if (def is GraphQLFragmentDefinition frag && frag.FragmentName.Name == name)
                return frag;
        }

        return null;
    }

    /// <summary>
    /// Searches the list for a AST node specified by name and returns first match.
    /// </summary>
    public static TNode? Find<TNode>(this ASTListNode<TNode> node, ROM name)
        where TNode : class, INamedNode
    {
        // DO NOT USE LINQ ON HOT PATH
        if (node.Items != null)
        {
            foreach (var item in node.Items)
            {
                if (item.Name == name)
                    return item;
            }
        }

        return null;
    }
}
