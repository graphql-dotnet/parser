namespace GraphQLParser.AST
{
    public class GraphQLDescription : ASTNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.Description;

        public ROM Value { get; set; }
    }
}
