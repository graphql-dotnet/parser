namespace GraphQLParser.AST
{
    public class GraphQLTypeExtensionDefinition : GraphQLTypeDefinition
    {
        public GraphQLObjectTypeDefinition? Definition { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.TypeExtensionDefinition;
    }
}
