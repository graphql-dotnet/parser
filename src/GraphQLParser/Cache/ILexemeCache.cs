namespace GraphQLParser
{
    /// <summary>
    /// Interface for caching token values. This cache allows you to reuse string values such
    /// as field names, argument values received when the <see cref="Lexer"/> is running.
    /// </summary>
    public interface ILexemeCache
    {
        string GetName(string source, int start, int end);

        string GetInt(string source, int start, int end);

        void Clear();
    }
}
