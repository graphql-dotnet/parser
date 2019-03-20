namespace GraphQLParser.AST
{
    public class GraphQLName : ASTNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.Name;

        public string Value { get; set; }
    }
}