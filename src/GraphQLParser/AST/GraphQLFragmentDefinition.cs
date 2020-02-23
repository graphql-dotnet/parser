namespace GraphQLParser.AST
{
    public class GraphQLFragmentDefinition : GraphQLInlineFragment, INamedNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.FragmentDefinition;

        public GraphQLName? Name { get; set; }
    }
}