using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLField : ASTNode, IHasDirectivesNode, IHasArgumentsNode, INamedNode
    {
        public GraphQLName? Alias { get; set; }

        public List<GraphQLArgument>? Arguments { get; set; }

        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Field;

        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }

        public GraphQLSelectionSet? SelectionSet { get; set; }
    }

    internal sealed class GraphQLFieldWithLocation : GraphQLField
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLFieldWithComment : GraphQLField
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLFieldFull : GraphQLField
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
