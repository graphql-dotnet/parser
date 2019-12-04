using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLUnionTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public IEnumerable<GraphQLDirective> Directives { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.UnionTypeDefinition;

        public IEnumerable<GraphQLNamedType> Types { get; set; }
    }
}