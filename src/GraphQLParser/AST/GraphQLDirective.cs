using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLDirective : ASTNode, INamedNode
    {
        public List<GraphQLArgument>? Arguments { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Directive;

        public GraphQLName? Name { get; set; }
    }
}
