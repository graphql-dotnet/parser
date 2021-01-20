using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLSelectionSet : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.SelectionSet;

        public List<ASTNode>? Selections { get; set; }
    }
}
