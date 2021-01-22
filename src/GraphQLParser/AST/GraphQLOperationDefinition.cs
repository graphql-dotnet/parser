using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLOperationDefinition : ASTNode, IHasDirectivesNode, INamedNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.OperationDefinition;

        public GraphQLName? Name { get; set; }

        public OperationType Operation { get; set; }

        public GraphQLSelectionSet? SelectionSet { get; set; }

        public List<GraphQLVariableDefinition>? VariableDefinitions { get; set; }
    }

    internal sealed class GraphQLOperationDefinitionFull : GraphQLOperationDefinition
    {
        private GraphQLLocation _location;
        private GraphQLComment? _comment;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }
}
