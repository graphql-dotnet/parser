using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.Field"/>.
    /// </summary>
    public class GraphQLField : ASTNode, IHasDirectivesNode, IHasArgumentsNode, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Field;

        public GraphQLAlias? Alias { get; set; }

        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }

        public List<GraphQLArgument>? Arguments { get; set; }

        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

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
