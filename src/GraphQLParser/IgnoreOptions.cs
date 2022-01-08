using System;

namespace GraphQLParser;

/// <summary>
/// Options to selectively ignore some information when parsing GraphQL document.
/// </summary>
[Flags]
public enum IgnoreOptions
{
    /// <summary>
    /// No information is ignored.
    /// </summary>
    None = 0,

    /// <summary>
    /// Specifies whether to ignore comments when parsing GraphQL document.
    /// </summary>
    Comments = 1,

    /// <summary>
    /// Specifies whether to ignore token locations when parsing GraphQL document.
    /// </summary>
    Locations = 2,

    /// <summary>
    /// Specifies whether to ignore comments and token locations when parsing GraphQL document.
    /// </summary>
    All = Comments | Locations,
}
