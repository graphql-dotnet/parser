namespace GraphQLParser.AST
{
    public class GraphQLVariable : GraphQLValue, INamedNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.Variable;

        public GraphQLName? Name { get; set; }
    }
}