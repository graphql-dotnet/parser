using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLFieldDefinition : GraphQLTypeDefinition, IHasDirectivesNode, INamedNode
    {
        public IEnumerable<GraphQLInputValueDefinition> Arguments { get; set; }

        public IEnumerable<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.FieldDefinition;

        public GraphQLName Name { get; set; }

        public GraphQLType Type { get; set; }
    }
}