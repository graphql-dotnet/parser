namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.FragmentDefinition"/>.
    /// </summary>
    public class GraphQLFragmentDefinition : GraphQLInlineFragment, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.FragmentDefinition;

        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }
    }

    internal sealed class GraphQLFragmentDefinitionWithLocation : GraphQLFragmentDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLFragmentDefinitionWithComment : GraphQLFragmentDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
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
