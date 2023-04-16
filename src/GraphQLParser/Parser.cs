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

    /// <summary>
    /// Generates AST based on input text.
    /// </summary>
    /// <typeparam name="T">Type of node to parse input text as.</typeparam>
    /// <param name="source">Input data as a sequence of characters.</param>
    /// <param name="options">Parser options.</param>
    /// <returns>AST (Abstract Syntax Tree) for GraphQL document.</returns>
    /// <exception cref="GraphQLSyntaxErrorException">In case when parser recursion depth exceeds <see cref="ParserOptions.MaxDepth"/>.</exception>
    /// <exception cref="GraphQLMaxDepthExceededException">In case of syntax error.</exception>
    /// <exception cref="NotSupportedException">The specified node type is unsupported.</exception>
    public static T Parse<T>(ROM source, ParserOptions options = default)
        where T : ASTNode
    {
        var context = new ParserContext(source, options);
        T result;
        if (typeof(T) == typeof(GraphQLDocument))
            result = (T)(object)context.ParseDocument();
        else if (typeof(T) == typeof(GraphQLValue))
            result = (T)(object)context.ParseValueLiteral(true);
        else if (typeof(T) == typeof(GraphQLArgument))
            result = (T)(object)context.ParseArgument();
        else if (typeof(T) == typeof(GraphQLArguments))
            result = (T)(object)context.ParseArguments();
        /* and so on */
        else
            throw new NotSupportedException();
        context.Expect(TokenKind.EOF);
        return result;
    }
}
