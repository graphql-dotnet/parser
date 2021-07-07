using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInputObjectTypeDefinition : GraphQLTypeDefinitionWithDescription, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        public List<GraphQLInputValueDefinition>? Fields { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeDefinition;
    }

    internal sealed class GraphQLInputObjectTypeDefinitionFull : GraphQLInputObjectTypeDefinition
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
