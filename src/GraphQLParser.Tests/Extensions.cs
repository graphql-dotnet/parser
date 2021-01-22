using System;
using System.Collections.Generic;
using System.Text;
using GraphQLParser.AST;

namespace GraphQLParser.Tests
{
    internal static class ParserTestExtensions
    {
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
    }
}
