using GraphQLParser.AST;
using GraphQLParser.Exceptions;

namespace GraphQLParser;

/// <summary>
/// Parser for GraphQL syntax.
/// </summary>
public static class Parser
{
    /// <summary>
    /// Generates AST based on input text.
    /// </summary>
    /// <param name="source">Input data as a sequence of characters.</param>
    /// <param name="options">Parser options.</param>
    /// <returns>AST (Abstract Syntax Tree) for GraphQL document.</returns>
    /// <exception cref="GraphQLSyntaxErrorException">In case when parser recursion depth exceeds <see cref="ParserOptions.MaxDepth"/>.</exception>
    /// <exception cref="GraphQLMaxDepthExceededException">In case of syntax error.</exception>
    public static GraphQLDocument Parse(ROM source, ParserOptions options = default)
        => new ParserContext(source, options).ParseDocument();
}
