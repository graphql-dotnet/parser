namespace GraphQLParser
{
    public interface ILexemeCache
    {
        string GetName(string source, int start, int end);

        string GetInt(string source, int start, int end);

        void Clear();
    }
}
