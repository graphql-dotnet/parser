using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLEnumValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.EnumValueDefinition;
    }
}
