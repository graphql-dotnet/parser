namespace GraphQLParser.AST
{
    public class GraphQLListType : GraphQLType
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ListType;

        public GraphQLType? Type { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"[{Type}]";
    }
}
