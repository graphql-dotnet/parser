namespace GraphQLParser.AST
{
    public class GraphQLNamedType : GraphQLType, INamedNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.NamedType;

        public GraphQLName? Name { get; set; }

        public override string ToString() => Name?.Value!;
    }
}