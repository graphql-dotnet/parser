namespace GraphQLParser.AST
{
    public abstract class GraphQLTypeDefinition : ASTNode, INamedNode
    {
        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }
    }
}
