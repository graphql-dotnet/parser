using System;

namespace GraphQLParser.AST
{
    public class GraphQLComment : ASTNode
    {
        public GraphQLComment(string comment)
        {
            Comment = comment;
        }

        public override ASTNodeKind Kind => ASTNodeKind.Comment;

        public string Comment { get; set; }
    }
}
