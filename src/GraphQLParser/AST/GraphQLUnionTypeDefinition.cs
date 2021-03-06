using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLUnionTypeDefinition : GraphQLTypeDefinitionWithDescription, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.UnionTypeDefinition;

        public List<GraphQLNamedType>? Types { get; set; }
    }

    internal sealed class GraphQLUnionTypeDefinitionFull : GraphQLUnionTypeDefinition
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
