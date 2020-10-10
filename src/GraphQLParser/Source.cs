namespace GraphQLParser
{
    public class Source : ISource
    {
        public Source(string body) : this(body, "GraphQL")
        {
        }

        public Source(string body, string name)
        {
            Name = name;
            Body = body ?? string.Empty;
        }

        public string Body { get; set; }

        public string Name { get; set; }
    }
}
