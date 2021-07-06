namespace GraphQLParser.AST
{
    public abstract class GraphQLTypeDefinition : ASTNode, INamedNode
    {
        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }
    }

    public abstract class GraphQLTypeDefinitionWithDescription : GraphQLTypeDefinition, IHasDescription
    {
        /// <summary>
        /// Description of the node
        /// </summary>
        public GraphQLDescription? Description { get; set; }
    }
}
