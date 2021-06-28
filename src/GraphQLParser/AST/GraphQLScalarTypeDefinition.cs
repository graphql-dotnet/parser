using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLScalarTypeDefinition : GraphQLTypeDefinitionWithDescription, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ScalarTypeDefinition;
    }

    internal sealed class GraphQLScalarTypeDefinitionFull : GraphQLScalarTypeDefinition
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
