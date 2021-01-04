namespace GraphQLParser.AST
{
    public class GraphQLListType : GraphQLType
    {
        public override ASTNodeKind Kind => ASTNodeKind.ListType;

        public GraphQLType? Type { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"[{Type}]";
    }
}
