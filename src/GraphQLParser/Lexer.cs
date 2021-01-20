namespace GraphQLParser
{
    /// <summary>
    /// Lexer for GraphQL syntax.
    /// </summary>
    public static class Lexer
    {
        /// <summary>
        /// Generates token based on input text.
        /// </summary>
        /// <param name="source">Input data as a sequence of characters.</param>
        /// <param name="start">The index in the source at which to start searching the token.</param>
        /// <returns>Found token.</returns>
        public static Token Lex(ROM source, int start = 0) => new LexerContext(source, start).GetToken();
    }
}
