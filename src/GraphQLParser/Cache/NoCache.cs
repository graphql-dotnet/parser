namespace GraphQLParser
{
    public sealed class NoCache : ILexemeCache
    {
        public string Get(string source, int start, int end)
        {
            return source.Substring(start, end - start);
        }

        public void Clear() { }
    }
}
