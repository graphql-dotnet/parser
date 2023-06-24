using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Options for <see cref="SDLSorter"/>.
/// </summary>
public class SDLSorterOptions : IASTVisitorContext, IComparer<ASTNode>, IComparer<DirectiveLocation>
{
    CancellationToken IASTVisitorContext.CancellationToken => default;

    /// <summary>
    /// Returns the <see cref="System.StringComparison"/> used by the comparer.
    /// </summary>
    public StringComparison StringComparison { get; } = StringComparison.InvariantCultureIgnoreCase;

    /// <summary>
    /// Initializes a new instances with the specified <see cref="System.StringComparison"/>.
    /// </summary>
    public SDLSorterOptions(StringComparison stringComparison)
    {
        StringComparison = stringComparison;
    }

    /// <summary>
    /// Returns a default instance which sorts using a culture-invariant case-insensitive string comparison.
    /// </summary>
    public static SDLSorterOptions Default { get; } = new SDLSorterOptions(StringComparison.InvariantCultureIgnoreCase);

    /// <inheritdoc cref="IComparer{T}.Compare(T, T)"/>
    public virtual int Compare(ASTNode x, ASTNode y)
    {
        int primaryComparison = Comparer<int>.Default.Compare(PrimarySort(x), PrimarySort(y));
        if (primaryComparison != 0)
            return primaryComparison;
        return SecondarySort(x).CompareTo(SecondarySort(y), StringComparison);

        static int PrimarySort(ASTNode node) => node switch
        {
            // comments go first
            GraphQLComment => 0,
            // sorting for selection sets
            GraphQLField => 1,
            GraphQLInlineFragment => 2,
            GraphQLFragmentSpread => 3,
            // sorting for executable documents
            GraphQLOperationDefinition op when op.Operation == OperationType.Query => 4,
            GraphQLOperationDefinition op when op.Operation == OperationType.Mutation => 5,
            GraphQLOperationDefinition op when op.Operation == OperationType.Subscription => 6,
            GraphQLFragmentDefinition => 7,
            // sorting for SDL documents
            GraphQLSchemaDefinition => 8,
            GraphQLDirectiveDefinition => 9,
            GraphQLTypeDefinition => 10,
            _ => 11 // sort other definitions together
        };

        static ReadOnlySpan<char> SecondarySort(ASTNode node) => node switch
        {
            GraphQLOperationDefinition x => x.Name?.Value ?? "", // although this implements INamedNode, it may be nullable here
            INamedNode x => x.Name.Value, // note: this includes values in map literals (GraphQLObjectField instances)
            GraphQLInlineFragment x => x.TypeCondition != null ? x.TypeCondition.Type.Name.Value : "",
            GraphQLFragmentSpread x => x.FragmentName.Name.Value,
            GraphQLRootOperationTypeDefinition x => x.Operation switch { OperationType.Query => "a", OperationType.Mutation => "b", _ => "c" },
            GraphQLVariableDefinition x => x.Variable.Name.Value,
            _ => "", // do not sort values, comments, etc
        };
    }

    /// <inheritdoc/>
    public int Compare(DirectiveLocation x, DirectiveLocation y)
        => Comparer<string>.Default.Compare(x.ToString(), y.ToString());

    int IComparer<ASTNode>.Compare(ASTNode? x, ASTNode? y) => Compare(x!, y!);
}
