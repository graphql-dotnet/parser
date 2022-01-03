using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.SchemaDefinition"/>.
    /// </summary>
    public class GraphQLSchemaDefinition : ASTNode, IHasDirectivesNode, IHasDescriptionNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.SchemaDefinition;

        /// <inheritdoc/>
        public GraphQLDescription? Description { get; set; }

        /// <inheritdoc/>
        public GraphQLDirectives? Directives { get; set; }

        public List<GraphQLRootOperationTypeDefinition>? OperationTypes { get; set; }
    }

    internal sealed class GraphQLSchemaDefinitionWithLocation : GraphQLSchemaDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLSchemaDefinitionWithComment : GraphQLSchemaDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
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
