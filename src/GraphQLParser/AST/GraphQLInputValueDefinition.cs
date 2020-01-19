using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInputValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public GraphQLValue DefaultValue { get; set; }

        public List<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.InputValueDefinition;

        public GraphQLType Type { get; set; }
    }
}