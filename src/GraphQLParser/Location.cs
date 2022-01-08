using System.Text.RegularExpressions;

namespace GraphQLParser;

/// <summary>
/// Provides the ability to decode a linear character position into a line and column number.
/// </summary>
public readonly struct Location
{
    private static readonly Regex _lineRegex = new("\r\n|[\n\r]", RegexOptions.ECMAScript);

    /// <summary>
    /// Creates location from a given sequence of characters and a linear character position.
    /// </summary>
    /// <param name="source">Input data as a sequence of characters.</param>
    /// <param name="position">Linear character position in the <paramref name="source"/>.</param>
    public Location(ROM source, int position)
    {
        Line = 1;
        Column = position + 1;

        if (position > 0)
        {
            var matches = _lineRegex.Matches((string)source); // TODO: heap allocation
            foreach (Match match in matches)
            {
                if (match.Index >= position)
                    break;

                ++Line;
                Column = position + 1 - (match.Index + matches[0].Length);
            }
        }
    }

    /// <summary>
    /// The column number on which the character is located.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// The line number on which the character is located.
    /// </summary>
    public int Line { get; }
}
