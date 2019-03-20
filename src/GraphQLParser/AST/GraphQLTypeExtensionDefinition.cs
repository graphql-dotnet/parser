namespace GraphQLParser.AST
{
    public class GraphQLTypeExtensionDefinition : GraphQLTypeDefinition
    {
        public GraphQLObjectTypeDefinition Definition { get; set; }

        public override ASTNodeKind Kind => ASTNodeKind.TypeExtensionDefinition;
    }
}