namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.UnionMemberTypes"/>.
    /// </summary>
    public class GraphQLUnionMemberTypes : ASTListNode<GraphQLNamedType>
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.UnionMemberTypes;
    }

    internal sealed class GraphQLUnionMemberTypesWithLocation : GraphQLUnionMemberTypes
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLUnionMemberTypesWithComment : GraphQLUnionMemberTypes
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLUnionMemberTypesFull : GraphQLUnionMemberTypes
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
