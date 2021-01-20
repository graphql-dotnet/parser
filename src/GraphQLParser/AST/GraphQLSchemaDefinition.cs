using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLSchemaDefinition : ASTNode, IHasDirectivesNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.SchemaDefinition;

        public List<GraphQLOperationTypeDefinition>? OperationTypes { get; set; }
    }
}
