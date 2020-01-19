using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLScalarTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.ScalarTypeDefinition;
    }
}