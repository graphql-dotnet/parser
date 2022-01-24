using System.Collections.Generic;
using System.IO;
using System.Threading;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Context used by <see cref="SDLWriter{TContext}"/> and <see cref="StructureWriter{TContext}"/>.
/// </summary>
public interface IWriteContext : IASTVisitorContext
{
    /// <summary>
    /// A writer to write document.
    /// </summary>
    TextWriter Writer { get; }

    /// <summary>
    /// Stack of AST nodes to track the current visitor position.
    /// </summary>
    Stack<ASTNode> Parents { get; }

    /// <summary>
    /// Tracks the current indent level.
    /// </summary>
    int IndentLevel { get; set; }
}

/// <summary>
/// Default implementation for <see cref="IWriteContext"/>.
/// </summary>
public class DefaultWriteContext : IWriteContext
{
    /// <summary>
    /// Creates an instance with the specified <see cref="TextWriter"/>.
    /// </summary>
    public DefaultWriteContext(TextWriter writer)
    {
        Writer = writer;
    }

    /// <inheritdoc/>
    public TextWriter Writer { get; }

    /// <inheritdoc/>
    public Stack<ASTNode> Parents { get; init; } = new Stack<ASTNode>();

    /// <inheritdoc/>
    public CancellationToken CancellationToken { get; init; }

    /// <inheritdoc/>
    public int IndentLevel { get; set; }
}
