using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLOperationDefinition : ASTNode, IHasDirectivesNode, INamedNode
    {
        public IEnumerable<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.OperationDefinition;

        public GraphQLName Name { get; set; }
        public OperationType Operation { get; set; }
        public GraphQLSelectionSet SelectionSet { get; set; }
        public IEnumerable<GraphQLVariableDefinition> VariableDefinitions { get; set; }
    }
}