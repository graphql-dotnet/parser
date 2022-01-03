namespace GraphQLParser.Exceptions
{
    /// <summary>
    /// An exception representing a GraphQL document syntax error.
    /// </summary>
    public class GraphQLSyntaxErrorException : GraphQLParserException
    {
        /// <summary>
        /// Initializes a new instance with the specified parameters.
        /// </summary>
        public GraphQLSyntaxErrorException(string description, ROM source, int location)
            : base(description, source, location)
        {
        }
    }
}
