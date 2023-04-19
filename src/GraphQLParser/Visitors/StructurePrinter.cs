using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Prints AST into the provided <see cref="TextWriter"/> as a hierarchy of node types.
/// </summary>
/// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
public class StructurePrinter<TContext> : ASTVisitor<TContext>
    where TContext : IPrintContext
{
    /// <summary>
    /// Creates visitor with default options.
    /// </summary>
    public StructurePrinter()
        : this(new StructurePrinterOptions())
    {
    }

    /// <summary>
    /// Creates visitor with the specified options.
    /// </summary>
    /// <param name="options">Visitor options.</param>
    public StructurePrinter(StructurePrinterOptions options)
    {
        Options = options;
    }

    /// <summary>
    /// Options used by visitor.
    /// </summary>
    public StructurePrinterOptions Options { get; }

    /// <inheritdoc/>
    public override async ValueTask VisitAsync(ASTNode? node, TContext context)
    {
        if (node == null)
            return;

        await WriteIndentAsync(context).ConfigureAwait(false);

        ++context.IndentLevel;
        context.Parents.Push(node);
        await context.WriteAsync(node.Kind.ToString()).ConfigureAwait(false); //ISSUE: allocation
        if (Options.PrintNames && node is GraphQLName name)
        {
            await context.WriteAsync(" [").ConfigureAwait(false);
            await context.WriteAsync(name.Value).ConfigureAwait(false);
            await context.WriteAsync("]").ConfigureAwait(false);
        }
        if (Options.PrintLocations)
        {
            await context.WriteAsync(" ").ConfigureAwait(false);
            await context.WriteAsync(node.Location.ToString()).ConfigureAwait(false); //ISSUE: allocation
        }
        await context.WriteLineAsync().ConfigureAwait(false);
        await base.VisitAsync(node, context).ConfigureAwait(false);
        _ = context.Parents.Pop();
        --context.IndentLevel;
    }

    private async ValueTask WriteIndentAsync(TContext context)
    {
        for (int i = 0; i < context.IndentLevel; ++i)
        {
            for (int j = 0; j < Options.IndentSize; ++j)
                await context.WriteAsync(" ").ConfigureAwait(false);
        }
    }
}

/// <inheritdoc/>
public class StructurePrinter : StructurePrinter<SDLPrinter.DefaultPrintContext>
{
    /// <inheritdoc/>
    public StructurePrinter()
        : base()
    {
    }

    /// <inheritdoc/>
    public StructurePrinter(StructurePrinterOptions options)
        : base(options)
    {
    }

    /// <inheritdoc cref="StructurePrinter{TContext}"/>
    public virtual ValueTask PrintAsync(ASTNode node, TextWriter writer, CancellationToken cancellationToken = default)
    {
        var context = new SDLPrinter.DefaultPrintContext(writer)
        {
            CancellationToken = cancellationToken,
        };
        return VisitAsync(node, context);
    }
}

/// <summary>
/// Options for <see cref="StructurePrinter{TContext}"/>.
/// </summary>
public class StructurePrinterOptions
{
    /// <summary>
    /// Print <see cref="GraphQLName.Value"/> into the output.
    /// </summary>
    public bool PrintNames { get; init; } = true;

    /// <summary>
    /// Print <see cref="ASTNode.Location"/> into the output.
    /// </summary>
    public bool PrintLocations { get; init; }

    /// <summary>
    /// The size of the horizontal indentation in spaces.
    /// </summary>
    public int IndentSize { get; init; } = 2;
}
