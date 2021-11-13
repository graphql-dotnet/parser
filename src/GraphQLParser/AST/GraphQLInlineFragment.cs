using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInlineFragment : ASTNode, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InlineFragment;

        public GraphQLTypeCondition? TypeCondition { get; set; }

        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        public GraphQLSelectionSet? SelectionSet { get; set; }
    }

    internal sealed class GraphQLInlineFragmentWithLocation : GraphQLInlineFragment
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLInlineFragmentWithComment : GraphQLInlineFragment
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLInlineFragmentFull : GraphQLInlineFragment
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
