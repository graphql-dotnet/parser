namespace GraphQLParser.AST
{
    public abstract class GraphQLTypeDefinition : ASTNode, INamedNode
    {
        public GraphQLName? Name { get; set; }
    }
}