using System.Globalization;
using System.Text;

namespace GraphQLParser.Exceptions;

/// <summary>
/// An exception representing a GraphQL document parsing error.
/// </summary>
public class GraphQLParserException : Exception
{
    private static readonly char[] NewlineSeparator = ['\n'];

    /// <summary>
    /// Error description.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Location of the symbol that caused the error.
    /// </summary>
    public Location Location { get; }

    /// <summary>
    /// Initializes a new instance with the specified parameters.
    /// </summary>
    public GraphQLParserException(string description, ROM source, int location)
        : this(description, source, Location.FromLinearPosition(source, location))
    {
    }

    private GraphQLParserException(string description, ReadOnlySpan<char> source, Location location)
        : base(ComposeMessage(description, source, location))
    {
        Description = description;
        Location = location;
    }

    //TODO: all this code can be rewritten to not allocate strings but is it worth it?
    private static string ComposeMessage(string description, ReadOnlySpan<char> source, Location location)
    {
        return $"Syntax Error GraphQL ({location.Line}:{location.Column}) {description}" +
            "\n" + HighlightSourceAtLocation(source, location);
    }

    private static string HighlightSourceAtLocation(ReadOnlySpan<char> source, Location location)
    {
        int line = location.Line;
        string prevLineNum = (line - 1).ToString(CultureInfo.InvariantCulture);
        string lineNum = line.ToString(CultureInfo.InvariantCulture);
        string nextLineNum = (line + 1).ToString(CultureInfo.InvariantCulture);
        int padLen = nextLineNum.Length;
        string[] lines = source
            .ToString()
            .Split(NewlineSeparator, StringSplitOptions.None)
            .Select(e => ReplaceWithUnicodeRepresentation(e))
            .ToArray();

        return
            (line >= 2 ? LeftPad(padLen, prevLineNum) + ": " + lines[line - 2] + "\n" : string.Empty) +
            LeftPad(padLen, lineNum) + ": " + lines[line - 1] + "\n" +
            LeftPad(1 + padLen + location.Column, string.Empty) + "^" + "\n" +
            (line < lines.Length ? LeftPad(padLen, nextLineNum) + ": " + lines[line] + "\n" : string.Empty);
    }

    private static string LeftPad(int length, string str)
    {
        string pad = string.Empty;

        for (int i = 0; i < length - str.Length; ++i)
            pad += " ";

        return pad + str;
    }

    private static string ReplaceWithUnicodeRepresentation(string str)
    {
        if (!HasReplacementCharacter(str))
            return str;

        var buffer = new StringBuilder(str.Length);

        foreach (char code in str)
        {
            if (IsReplacementCharacter(code))
            {
                buffer.Append("\\u").Append(((int)code).ToString("D4", CultureInfo.InvariantCulture));
            }
            else
            {
                buffer.Append(code);
            }
        }

        return buffer.ToString();
    }

    private static bool HasReplacementCharacter(string str)
    {
        foreach (char code in str)
        {
            if (IsReplacementCharacter(code))
                return true;
        }

        return false;
    }

    private static bool IsReplacementCharacter(char code) => code < 0x0020 && code != 0x0009 && code != 0x000A && code != 0x000D;
}
