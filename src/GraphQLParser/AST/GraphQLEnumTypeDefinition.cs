using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLEnumTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.EnumTypeDefinition;

        public List<GraphQLEnumValueDefinition> Values { get; set; }
    }
}