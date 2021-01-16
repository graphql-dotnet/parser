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

        public GraphQLDocument Parse(ISource source) => Parse(source, false);

        public GraphQLDocument Parse(ISource source, bool ignoreComments)
        {
            var context = new ParserContext(source, _lexer, ignoreComments);
            return context.Parse();
        }
    }
}
