namespace GraphQLParser.Tests;

internal static class ParserTestExtensions
{
    internal static string ReadGraphQLFile(this string name) => File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", name + ".graphql"));

    /// <summary>
    /// Generates token based on input text.
    /// </summary>
    /// <param name="source">Input data as a string.</param>
    /// <param name="start">The index in the source at which to start searching the token.</param>
    /// <returns></returns>
    public static Token Lex(this string source, int start = 0) => Lexer.Lex(source, start);

    /// <summary>
    /// Generates AST based on input text.
    /// </summary>
    /// <param name="source">Input data as a string.</param>
    /// <param name="options">Parser options.</param>
    /// <returns>AST (Abstract Syntax Tree) for GraphQL document.</returns>
    public static GraphQLDocument Parse(this string source, ParserOptions options = default) => Parser.Parse(source, options);

    /// <summary>
    /// Generates AST based on input text.
    /// </summary>
    /// <typeparam name="T">Type of node to parse input text as.</typeparam>
    /// <param name="source">Input data as a sequence of characters.</param>
    /// <param name="options">Parser options.</param>
    /// <returns>AST (Abstract Syntax Tree) for GraphQL node.</returns>
    public static T Parse<T>(this string source, ParserOptions options = default)
        where T : ASTNode
        => Parser.Parse<T>(source, options);
}
