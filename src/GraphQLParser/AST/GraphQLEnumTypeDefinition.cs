using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLEnumTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode, INamedNode
    {
        public IEnumerable<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.EnumTypeDefinition;

        public GraphQLName Name { get; set; }

        public IEnumerable<GraphQLEnumValueDefinition> Values { get; set; }
    }
}