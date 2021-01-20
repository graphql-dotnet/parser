using GraphQLParser.AST;

namespace GraphQLParser
{
    /// <summary>
    /// Parser for GraphQL syntax.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Generates AST based on input text.
        /// </summary>
        /// <param name="source">Input data as a sequence of characters.</param>
        /// <param name="ignoreComments">Specifies whether to ignore comments when parsing GraphQL document. By default, all comments are ignored.</param>
        /// <returns>AST (Abstract Syntax Tree) for GraphQL document.</returns>
        public static GraphQLDocument Parse(ROM source, bool ignoreComments = true) => new ParserContext(source, ignoreComments).Parse();
    }
}
