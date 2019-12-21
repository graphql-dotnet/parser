using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLFragmentSpread : ASTNode, IHasDirectivesNode, INamedNode
    {
        public List<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.FragmentSpread;

        public GraphQLName Name { get; set; }
    }
}