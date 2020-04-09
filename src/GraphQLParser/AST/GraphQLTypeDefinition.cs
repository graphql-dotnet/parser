namespace GraphQLParser.AST
{
    public abstract class GraphQLTypeDefinition : ASTNode, INamedNode, IHaveDescription
    {
        public GraphQLName? Name { get; set; }

        public GraphQLDescription? Description { get; set; }
    }
}
