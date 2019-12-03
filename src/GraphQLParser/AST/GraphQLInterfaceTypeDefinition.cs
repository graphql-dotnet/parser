using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInterfaceTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public IEnumerable<GraphQLDirective> Directives { get; set; }

        public IEnumerable<GraphQLFieldDefinition> Fields { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.InterfaceTypeDefinition;
    }
}