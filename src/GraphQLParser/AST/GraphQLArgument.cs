namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.Argument"/>.
    /// </summary>
    public class GraphQLArgument : ASTNode, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Argument;

        /// <inheritdoc/>
        public GraphQLName Name { get; set; } = null!;

        public GraphQLValue? Value { get; set; }
    }

    internal sealed class GraphQLArgumentWithLocation : GraphQLArgument
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLArgumentWithComment : GraphQLArgument
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLArgumentFull : GraphQLArgument
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
