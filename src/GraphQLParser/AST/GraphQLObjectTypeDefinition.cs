using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        public List<GraphQLFieldDefinition>? Fields { get; set; }

        public List<GraphQLNamedType>? Interfaces { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ObjectTypeDefinition;
    }

    internal sealed class GraphQLObjectTypeDefinitionFull : GraphQLObjectTypeDefinition
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
