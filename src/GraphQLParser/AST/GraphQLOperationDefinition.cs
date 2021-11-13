using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.OperationDefinition"/>.
    /// </summary>
    public class GraphQLOperationDefinition : ASTNode, IHasDirectivesNode, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.OperationDefinition;

        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }

        public OperationType Operation { get; set; }

        public GraphQLSelectionSet? SelectionSet { get; set; }

        public List<GraphQLVariableDefinition>? VariableDefinitions { get; set; }
    }

    internal sealed class GraphQLOperationDefinitionWithLocation : GraphQLOperationDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLOperationDefinitionWithComment : GraphQLOperationDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
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
