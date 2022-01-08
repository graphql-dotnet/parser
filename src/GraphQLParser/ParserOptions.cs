namespace GraphQLParser;

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
    /// Depth is calculated in terms of AST nodes.
    /// <br/>
    /// Defaults to 128 if not set.
    /// Minimum value is 1.
    /// </summary>
    public int? MaxDepth { get; set; }
}
