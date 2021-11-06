namespace GraphQLParser.AST
{
    public abstract class GraphQLTypeDefinition : ASTNode, INamedNode, IHasDescriptionNode
    {
        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }

        /// <inheritdoc/>
        public GraphQLDescription? Description { get; set; }
    }
}
