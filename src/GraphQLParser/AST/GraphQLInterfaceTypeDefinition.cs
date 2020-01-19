using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInterfaceTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLDirective> Directives { get; set; }

        public List<GraphQLFieldDefinition> Fields { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.InterfaceTypeDefinition;
    }
}