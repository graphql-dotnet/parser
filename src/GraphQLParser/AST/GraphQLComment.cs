namespace GraphQLParser.AST
{
    public class GraphQLComment : ASTNode
    {
        public GraphQLComment(string text)
        {
            Text = text;
        }

        public override ASTNodeKind Kind => ASTNodeKind.Comment;

        public string Text { get; set; }
    }
}
