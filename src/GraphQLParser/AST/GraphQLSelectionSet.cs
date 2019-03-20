using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLSelectionSet : ASTNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.SelectionSet;

        public IEnumerable<ASTNode> Selections { get; set; }
    }
}