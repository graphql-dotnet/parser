namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.TypeCondition"/>.
    /// </summary>
    public class GraphQLTypeCondition : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.TypeCondition;

        /// <summary>
        /// Type to which this condition is applied.
        /// </summary>
        public GraphQLNamedType? Type { get; set; }
    }

    internal sealed class GraphQLTypeConditionWithLocation : GraphQLTypeCondition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLTypeConditionWithComment : GraphQLTypeCondition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLTypeConditionFull : GraphQLTypeCondition
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
