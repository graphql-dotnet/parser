namespace GraphQLParser.AST
{
    public class GraphQLArgument : ASTNode, INamedNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.Argument;

        public GraphQLName? Name { get; set; }

        public GraphQLValue? Value { get; set; }
    }
}