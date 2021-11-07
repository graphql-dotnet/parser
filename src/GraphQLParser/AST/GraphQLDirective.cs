using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents a directive, applied to some GraphQL element.
    /// </summary>
    public class GraphQLDirective : ASTNode, INamedNode
    {
        public List<GraphQLArgument>? Arguments { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Directive;

        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }
    }

    internal sealed class GraphQLDirectiveWithLocation : GraphQLDirective
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLDirectiveWithComment : GraphQLDirective
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLDirectiveFull : GraphQLDirective
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
