namespace GraphQLParser.AST
{
    public class GraphQLDescription : ASTNode
    {
        public GraphQLDescription(string value)
        {
            Value = value;
        }

        public override ASTNodeKind Kind => ASTNodeKind.Description;

        public string Value { get; set; }
    }
}
