using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLUnionTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.UnionTypeDefinition;

        public List<GraphQLNamedType>? Types { get; set; }
    }
}
