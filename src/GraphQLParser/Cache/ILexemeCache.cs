namespace GraphQLParser
{
    public interface ILexemeCache
    {
        string Get(string source, int start, int end);

        void Clear();
    }
}
