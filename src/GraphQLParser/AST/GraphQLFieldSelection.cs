using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLFieldSelection : ASTNode, IHasDirectivesNode, INamedNode
    {
        public GraphQLName? Alias { get; set; }

        public List<GraphQLArgument>? Arguments { get; set; }

        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Field;

        public GraphQLName? Name { get; set; }

        public GraphQLSelectionSet? SelectionSet { get; set; }
    }
}
