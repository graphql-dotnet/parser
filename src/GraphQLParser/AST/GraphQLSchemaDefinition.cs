using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLSchemaDefinition : ASTNode, IHasDirectivesNode, IHasDescriptionNode
    {
        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public GraphQLDescription? Description { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.SchemaDefinition;

        public List<GraphQLOperationTypeDefinition>? OperationTypes { get; set; }
    }

    internal sealed class GraphQLSchemaDefinitionFull : GraphQLSchemaDefinition
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
