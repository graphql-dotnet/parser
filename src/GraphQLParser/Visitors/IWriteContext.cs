using System.Collections.Generic;
using System.IO;

namespace GraphQLParser.Visitors;

/// <summary>
/// Context used by <see cref="SDLWriter{TContext}"/> and <see cref="StructureWriter{TContext}"/>.
/// </summary>
public interface IWriteContext : INodeVisitorContext
{
    /// <summary>
    /// A writer to write document.
    /// </summary>
    TextWriter Writer { get; }

    /// <summary>
    /// Stack of AST nodes to track the current visitor position.
    /// </summary>
    Stack<AST.ASTNode> Parents { get; }

    /// <summary>
    /// Tracks the current indent level.
    /// </summary>
    int IndentLevel { get; set; }
}
