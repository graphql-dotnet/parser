namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.NamedType"/>.
    /// </summary>
    public class GraphQLNamedType : GraphQLType, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.NamedType;

        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Name?.Value.ToString()!;
    }

    internal sealed class GraphQLNamedTypeWithLocation : GraphQLNamedType
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLNamedTypeWithComment : GraphQLNamedType
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLNamedTypeFull : GraphQLNamedType
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
