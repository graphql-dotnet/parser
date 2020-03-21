namespace GraphQLParser
{
    /// <summary>
    /// <see cref="ILexemeCache"/> implementation without caching. It is used by default.
    /// </summary>
    public sealed class NoCache : ILexemeCache
    {
        public static readonly NoCache Instance = new NoCache();

        public string GetName(string source, int start, int end) => source.Substring(start, end - start);

        public string GetInt(string source, int start, int end) => source.Substring(start, end - start);

        public void Clear() { }
    }
}
