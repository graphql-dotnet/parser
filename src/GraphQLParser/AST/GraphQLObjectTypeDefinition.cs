using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHaveDescription
    {
        public List<GraphQLDirective>? Directives { get; set; }

        public List<GraphQLFieldDefinition>? Fields { get; set; }

        public List<GraphQLNamedType>? Interfaces { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.ObjectTypeDefinition;
    }
}
