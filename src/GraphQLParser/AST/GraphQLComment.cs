using System;

namespace GraphQLParser.AST
{
    public class GraphQLComment : ASTNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.Comment;

        public ReadOnlyMemory<char> Text { get; set; }
    }
}
