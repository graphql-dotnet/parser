namespace GraphQLParser.AST
{
    public abstract class GraphQLTypeDefinition : ASTNode, INamedNode
    {
        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }
    }

    public abstract class GraphQLTypeDefinitionWithDescription : GraphQLTypeDefinition
    {
        /// <summary>
        /// Description of the node as represented by a nested node.
        /// </summary>
        public GraphQLScalarValue? Description { get; set; }
    }
}
