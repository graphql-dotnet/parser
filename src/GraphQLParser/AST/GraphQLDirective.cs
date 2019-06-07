using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLDirective : ASTNode, INamedNode
    {
        public IEnumerable<GraphQLArgument> Arguments { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.Directive;

        public GraphQLName Name { get; set; }
    }
}