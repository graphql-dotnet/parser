using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLEnumValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public IEnumerable<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.EnumValueDefinition;
    }
}