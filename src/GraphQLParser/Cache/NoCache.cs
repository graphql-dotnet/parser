namespace GraphQLParser
{
    public sealed class NoCache : ILexemeCache
    {
        public static readonly NoCache Instance = new NoCache();

        public string Get(string source, int start, int end)
        {
            return source.Substring(start, end - start);
        }

        public void Clear() { }
    }
}
