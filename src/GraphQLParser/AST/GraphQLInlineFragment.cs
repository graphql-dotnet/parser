using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInlineFragment : ASTNode, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InlineFragment;

        public GraphQLSelectionSet? SelectionSet { get; set; }

        public GraphQLNamedType? TypeCondition { get; set; }
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
