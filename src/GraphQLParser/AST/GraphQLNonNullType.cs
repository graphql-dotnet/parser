namespace GraphQLParser.AST
{
    public class GraphQLNonNullType : GraphQLType
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.NonNullType;

        public GraphQLType? Type { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Type + "!";
    }

    internal sealed class GraphQLNonNullTypeWithLocation : GraphQLNonNullType
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLNonNullTypeWithComment : GraphQLNonNullType
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLNonNullTypeFull : GraphQLNonNullType
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
