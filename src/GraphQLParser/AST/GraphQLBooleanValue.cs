namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.BooleanValue"/>.
    /// </summary>
    public class GraphQLBooleanValue : GraphQLValue
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.BooleanValue;

        /// <summary>
        /// Value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Value { get; set; }
    }

    internal sealed class GraphQLBooleanValueWithLocation : GraphQLBooleanValue
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLBooleanValueWithComment : GraphQLBooleanValue
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLBooleanValueFull : GraphQLBooleanValue
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
