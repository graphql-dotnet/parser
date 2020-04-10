using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLSchemaDefinition : ASTNode, IHasDirectivesNode, IHasDescriptionNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.SchemaDefinition;

        public List<GraphQLOperationTypeDefinition>? OperationTypes { get; set; }

        public GraphQLDescription? Description { get; set; }
    }
}
