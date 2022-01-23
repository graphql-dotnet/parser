using System.IO;
using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Prints AST into the provided <see cref="TextWriter"/> as a hierarchy of node types.
/// </summary>
/// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
public class StructureWriter<TContext> : DefaultNodeVisitor<TContext>
    where TContext : IWriteContext
{
    /// <summary>
    /// Creates visitor with the specified options.
    /// </summary>
    /// <param name="options">Visitor options.</param>
    public StructureWriter(StructureWriterOptions options)
    {
        Options = options;
    }

    private StructureWriterOptions Options { get; }

    /// <inheritdoc/>
    public override async ValueTask VisitAsync(ASTNode? node, TContext context)
    {
        if (node == null)
            return;

        for (int i = 0; i < context.Parents.Count; ++i)
            await context.WriteAsync("  ").ConfigureAwait(false);

        context.Parents.Push(node);
        await context.WriteAsync(node.Kind.ToString()).ConfigureAwait(false);
        if (Options.WriteNames && node is GraphQLName name)
        {
            await context.WriteAsync(" [").ConfigureAwait(false);
            await context.WriteAsync(name.Value).ConfigureAwait(false);
            await context.WriteAsync("]").ConfigureAwait(false);
        }
        if (Options.WriteLocations)
        {
            await context.WriteAsync(" ").ConfigureAwait(false);
            await context.WriteAsync(node.Location.ToString()).ConfigureAwait(false); //TODO: allocations
        }
        await context.WriteLineAsync().ConfigureAwait(false);
        await base.VisitAsync(node, context).ConfigureAwait(false);
        _ = context.Parents.Pop();
    }
}

/// <summary>
/// Options for <see cref="StructureWriter{TContext}"/>.
/// </summary>
public class StructureWriterOptions
{
    /// <summary>
    /// Write <see cref="GraphQLName.Value"/> into the output.
    /// </summary>
    public bool WriteNames { get; set; } = true;

    /// <summary>
    /// Write <see cref="ASTNode.Location"/> into the output.
    /// </summary>
    public bool WriteLocations { get; set; }
}
