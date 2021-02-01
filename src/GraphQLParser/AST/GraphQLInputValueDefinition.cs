using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInputValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public GraphQLValue? DefaultValue { get; set; }

        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InputValueDefinition;

        public GraphQLType? Type { get; set; }
    }

    internal sealed class GraphQLInputValueDefinitionFull : GraphQLInputValueDefinition
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
