namespace GraphQLParser.AST
{
    public class GraphQLFragmentDefinition : GraphQLInlineFragment, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.FragmentDefinition;

        public GraphQLName? Name { get; set; }
    }
}
