namespace GraphQLParser.AST
{
    using System.Collections.Generic;

    public class GraphQLInputObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode, INamedNode
    {
        public IEnumerable<GraphQLDirective> Directives { get; set; }

        public IEnumerable<GraphQLInputValueDefinition> Fields { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeDefinition;

        public GraphQLName Name { get; set; }
    }
}