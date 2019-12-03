using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLEnumTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public IEnumerable<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.EnumTypeDefinition;

        public IEnumerable<GraphQLEnumValueDefinition> Values { get; set; }
    }
}