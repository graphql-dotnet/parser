using GraphQLParser.AST;

namespace GraphQLParser
{
    public class Parser
    {
        private readonly ILexer _lexer;

        public Parser(ILexer lexer)
        {
            _lexer = lexer;
        }

        public GraphQLDocument Parse(ISource source)
        {
            var context = new ParserContext(source, _lexer);
            return context.Parse();
        }
    }
}
