using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInputObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        public List<GraphQLInputValueDefinition>? Fields { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeDefinition;
    }
}
