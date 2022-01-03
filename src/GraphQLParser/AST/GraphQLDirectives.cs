using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.Directives"/>.
    /// </summary>
    public class GraphQLDirectives : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Directives;

        public List<GraphQLDirective> Items { get; set; } = null!;
    }

    internal sealed class GraphQLDirectivesWithLocation : GraphQLDirectives
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLDirectivesWithComment : GraphQLDirectives
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLDirectivesFull : GraphQLDirectives
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
