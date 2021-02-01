namespace GraphQLParser.AST
{
    public class GraphQLOperationTypeDefinition : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.OperationTypeDefinition;

        public OperationType Operation { get; set; }

        public GraphQLNamedType? Type { get; set; }
    }

    internal sealed class GraphQLOperationTypeDefinitionFull : GraphQLOperationTypeDefinition
    {
        private GraphQLLocation _location;
        //private GraphQLComment? _comment;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }

        // TODO: this property is not set anywhere (yet), so it makes no sense to create a field for it
        //public override GraphQLComment? Comment
        //{
        //    get => _comment;
        //    set => _comment = value;
        //}
    }
}
