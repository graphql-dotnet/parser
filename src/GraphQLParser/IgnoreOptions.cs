namespace GraphQLParser
{
    /// <summary>
    /// Options to selectively ignore some information when parsing GraphQL document.
    /// </summary>
    public enum IgnoreOptions
    {
        /// <summary>
        /// Specifies whether to ignore comments when parsing GraphQL document.
        /// </summary>
        IgnoreComments = 0,

        /// <summary>
        /// Specifies whether to ignore comments and token locations when parsing GraphQL document.
        /// </summary>
        IgnoreCommentsAndLocations = 1,

        /// <summary>
        /// No information is ignored.
        /// </summary>
        None = 2
    }
}
