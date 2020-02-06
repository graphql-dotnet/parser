namespace GraphQLParser.AST
{
    using System.Collections.Generic;

    public class GraphQLDirectiveDefinition : GraphQLTypeDefinition
    {
        public List<GraphQLInputValueDefinition>? Arguments { get; set; }

        public List<GraphQLInputValueDefinition>? Definitions { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.DirectiveDefinition;

        public List<GraphQLName>? Locations { get; set; }

        public bool Repeatable { get; set; }
    }
}