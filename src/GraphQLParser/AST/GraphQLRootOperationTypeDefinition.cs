namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.RootOperationTypeDefinition"/>.
    /// </summary>
    public class GraphQLRootOperationTypeDefinition : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.RootOperationTypeDefinition;

        /// <summary>
        /// Kind of operation: query, mutation or subscription.
        /// </summary>
        public OperationType Operation { get; set; }

        /// <summary>
        /// Type of this root operation.
        /// </summary>
        public GraphQLNamedType? Type { get; set; }
    }

    internal sealed class GraphQLRootOperationTypeDefinitionWithLocation : GraphQLRootOperationTypeDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLRootOperationTypeDefinitionWithComment : GraphQLRootOperationTypeDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLRootOperationTypeDefinitionFull : GraphQLRootOperationTypeDefinition
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
