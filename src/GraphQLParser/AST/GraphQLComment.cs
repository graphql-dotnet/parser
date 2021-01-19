namespace GraphQLParser.AST
{
    public class GraphQLComment : ASTNode
    {
        private ROM _text;
        private string? _textString;

        public override ASTNodeKind Kind => ASTNodeKind.Comment;

        public ROM Text
        {
            get => _text;
            set
            {
                _text = value;
                _textString = null;
            }
        }

        public string TextString => _textString ??= (string)Text;
    }
}
