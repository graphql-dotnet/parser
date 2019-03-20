namespace GraphQLParser.Exceptions
{
    using System;
    using System.Linq;
    using System.Text;

    public class GraphQLSyntaxErrorException : Exception
    {
        public GraphQLSyntaxErrorException(string description, ISource source, int location)
            : base(ComposeMessage(description, source, location))
        {
        }

        private static string ComposeMessage(string description, ISource source, int loc)
        {
            var location = new Location(source, loc);

            return $"Syntax Error GraphQL ({location.Line}:{location.Column}) {description}" +
                "\n" + HighlightSourceAtLocation(source, location);
        }

        private static string HighlightSourceAtLocation(ISource source, Location location)
        {
            var line = location.Line;
            var prevLineNum = (line - 1).ToString();
            var lineNum = line.ToString();
            var nextLineNum = (line + 1).ToString();
            var padLen = nextLineNum.Length;
            var lines = source.Body
                .Split(new string[] { "\n" }, StringSplitOptions.None)
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

            for (var i = 0; i < length - str.Length; i++)
                pad += " ";

            return pad + str;
        }

        private static string ReplaceWithUnicodeRepresentation(string str)
        {
            if (!HasReplacementCharacter(str))
                return str;

            var buffer = new StringBuilder(str.Length);

            foreach (var code in str)
            {
                if (IsReplacementCharacter(code))
                {
                    buffer.Append(GetUnicodeRepresentation(code));
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
            foreach (var code in str)
            {
                if (IsReplacementCharacter(code))
                    return true;
            }

            return false;
        }

        private static bool IsReplacementCharacter(char code) => code < 0x0020 && code != 0x0009 && code != 0x000A && code != 0x000D;

        private static string GetUnicodeRepresentation(char code)
        {
            switch (code)
            {
                case '\0':
                    return "\\u0000";
                default:
                    return "\\u" + ((int)code).ToString("D4");
            }
        }
    }
}