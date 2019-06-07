namespace GraphQLParser
{
    using System.Text.RegularExpressions;

    public class Location
    {
        public Location(ISource source, int position)
        {
            var lineRegex = new Regex("\r\n|[\n\r]", RegexOptions.ECMAScript);
            Line = 1;
            Column = position + 1;

            var matches = lineRegex.Matches(source.Body);
            foreach (Match match in matches)
            {
                if (match.Index >= position)
                    break;

                Line++;
                Column = position + 1 - (match.Index + matches[0].Length);
            }
        }

        public int Column { get; private set; }

        public int Line { get; private set; }
    }
}