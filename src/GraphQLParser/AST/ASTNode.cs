namespace GraphQLParser.AST
{
    public abstract class ASTNode
    {
        public abstract ASTNodeKind Kind { get; }

        public GraphQLLocation Location { get; set; }

        public GraphQLComment Comment { get; set; }
    }
}