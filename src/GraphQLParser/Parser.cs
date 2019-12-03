namespace GraphQLParser
{
    using GraphQLParser.AST;

    public class Parser
    {
        private readonly ILexer lexer;

        public Parser(ILexer lexer)
        {
            this.lexer = lexer;
        }

        public GraphQLDocument Parse(ISource source)
        {
            using var context = new ParserContext(source, lexer);
            return context.Parse();
        }
    }
}