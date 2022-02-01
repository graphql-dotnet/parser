using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Context used by <see cref="SDLPrinter{TContext}"/> and <see cref="StructurePrinter{TContext}"/>.
/// </summary>
public interface IPrintContext : IASTVisitorContext
{
    /// <summary>
    /// A text writer to print document.
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

    /// <summary>
    /// Indicates whether last GraphQL AST definition node (executable definition,
    /// type system definition or type system extension) from printed document was
    /// actualy printed. This property is required to properly print vertical
    /// indents between definitions.
    /// </summary>
    bool LastDefinitionPrinted { get; set; }
}
