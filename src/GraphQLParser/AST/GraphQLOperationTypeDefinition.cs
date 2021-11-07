namespace GraphQLParser.AST
{
    public class GraphQLOperationTypeDefinition : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.OperationTypeDefinition;

        public OperationType Operation { get; set; }

        public GraphQLNamedType? Type { get; set; }
    }

    internal sealed class GraphQLOperationTypeDefinitionWithLocation : GraphQLOperationTypeDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLOperationTypeDefinitionWithComment : GraphQLOperationTypeDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLOperationTypeDefinitionFull : GraphQLOperationTypeDefinition
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
