namespace GraphQLParser.AST
{
    public class GraphQLArgument : ASTNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.Argument;

        public GraphQLName Name { get; set; }

        public GraphQLValue Value { get; set; }
    }
}