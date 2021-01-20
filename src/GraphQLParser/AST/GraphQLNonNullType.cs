namespace GraphQLParser.AST
{
    public class GraphQLNonNullType : GraphQLType
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.NonNullType;

        public GraphQLType? Type { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Type + "!";
    }
}
