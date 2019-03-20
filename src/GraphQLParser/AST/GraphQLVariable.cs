namespace GraphQLParser.AST
{
    public class GraphQLVariable : GraphQLValue
    {
        public override ASTNodeKind Kind => ASTNodeKind.Variable;

        public GraphQLName Name { get; set; }
    }
}