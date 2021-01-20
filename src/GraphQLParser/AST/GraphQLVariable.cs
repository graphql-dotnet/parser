namespace GraphQLParser.AST
{
    public class GraphQLVariable : GraphQLValue, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Variable;

        public GraphQLName? Name { get; set; }
    }
}
