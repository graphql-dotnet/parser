namespace GraphQLParser.AST
{
    public class GraphQLFragmentDefinition : GraphQLInlineFragment, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.FragmentDefinition;

        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }
    }

    internal sealed class GraphQLFragmentDefinitionFull : GraphQLFragmentDefinition
    {
        private GraphQLLocation _location;
        private GraphQLComment? _comment;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }
}
