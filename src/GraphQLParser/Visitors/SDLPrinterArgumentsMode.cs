namespace GraphQLParser.Visitors;

/// <summary>
/// Defines how to print arguments definitions.
/// </summary>
public enum SDLPrinterArgumentsMode
{
    /// <summary>
    /// Print argument on new line only if it requires new line, i.e. has comment or description.
    /// Otherwise argument shares the same line with previous argument.
    /// </summary>
    None,

    /// <summary>
    /// Print all arguments on new line if any argument requires new line, i.e. has comment or description.
    /// This is default mode used by <see cref="SDLPrinterOptions"/>.
    /// </summary>
    PreferNewLine,

    /// <summary>
    /// Always print all arguments on new line.
    /// </summary>
    ForceNewLine,
}
