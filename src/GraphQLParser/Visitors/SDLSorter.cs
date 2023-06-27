using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Sorts AST nodes in the specified document.
/// </summary>
public sealed class SDLSorter : ASTVisitor<SDLSorterOptions>
{
    private static readonly SDLSorter _sorter = new();

    private SDLSorter()
    {
    }

    /// <summary>
    /// Sorts the specified document or node tree.
    /// Nodes that have the same sort order will be retain their relative position.
    /// </summary>
    public static void Sort(ASTNode node, SDLSorterOptions? options = null)
#pragma warning disable CA2012 // Use ValueTasks correctly
        => _sorter.VisitAsync(node, options ?? SDLSorterOptions.Default).GetAwaiter().GetResult();
#pragma warning restore CA2012 // Use ValueTasks correctly

    /// <inheritdoc/>
    protected override ValueTask VisitAsync<T>(List<T>? nodes, SDLSorterOptions context)
    {
        if (nodes == null)
            return default;

        // do not use List<T>.Sort as it is an unstable sorting algorithm
        // a stable sorting algorithm is necessary for (a) lists of literal values (b) comments
        StableSort(nodes, context);
        return base.VisitAsync(nodes, context);
    }

    /// <inheritdoc/>
    protected override ValueTask VisitDirectiveLocationsAsync(GraphQLDirectiveLocations directiveLocations, SDLSorterOptions context)
    {
        StableSort(directiveLocations.Items, context);
        return base.VisitDirectiveLocationsAsync(directiveLocations, context);
    }

    /// <summary>
    /// Performs a stable sort on the list.
    /// Uses a 'bubble sort' sorting algorithm if there are 20 or less items;
    /// otherwise uses LINQ to sort, which is a much faster algorithm when
    /// there are a greater quantity of items, at the cost of higher memory requirements.
    /// </summary>
    private static void StableSort<T>(List<T> list, IComparer<T> comparer)
    {
        var n = list.Count;
        if (n > 20)
        {
            var list2 = list.OrderBy(static x => x, comparer).ToArray();
            for (int i = 0; i < list2.Length; i++)
            {
                list[i] = list2[i];
            }
            return;
        }
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (comparer.Compare(list[j], list[j + 1]) > 0)
                {
                    var temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
            }
        }
    }
}
