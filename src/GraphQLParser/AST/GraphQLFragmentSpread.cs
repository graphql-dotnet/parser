using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLFragmentSpread : ASTNode, IHasDirectivesNode, INamedNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.FragmentSpread;

        public GraphQLName? Name { get; set; }
    }

    internal sealed class GraphQLFragmentSpreadFull : GraphQLFragmentSpread
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
