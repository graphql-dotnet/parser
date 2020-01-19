using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLFieldDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLInputValueDefinition> Arguments { get; set; }

        public List<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.FieldDefinition;

        public GraphQLType Type { get; set; }
    }
}