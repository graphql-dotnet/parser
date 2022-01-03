namespace GraphQLParser
{
    /// <summary>
    /// Parser options.
    /// </summary>
    public struct ParserOptions
    {
        /// <summary>
        /// Options to selectively ignore some information when parsing GraphQL document.
        /// By default, all comments are ignored.
        /// </summary>
        public IgnoreOptions Ignore { get; set; }

        /// <summary>
        /// Maximum allowed recursion depth during parsing.
        /// Defaults to 64 if not set.
        /// Minimum value is 1.
        /// </summary>
        public int? MaxDepth { get; set; }
    }
}
