namespace GraphQLParser.AST
{
    public class GraphQLOperationTypeDefinition : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.OperationTypeDefinition;

        public OperationType Operation { get; set; }

        public GraphQLNamedType? Type { get; set; }
    }
}
