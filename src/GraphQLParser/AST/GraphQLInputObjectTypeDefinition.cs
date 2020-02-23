namespace GraphQLParser.AST
{
    using System.Collections.Generic;

    public class GraphQLInputObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        public List<GraphQLInputValueDefinition>? Fields { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeDefinition;
    }
}