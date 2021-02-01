namespace GraphQLParser.AST
{
    public class GraphQLObjectField : ASTNode, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ObjectField;

        public GraphQLName? Name { get; set; }

        public GraphQLValue? Value { get; set; }
    }

    internal sealed class GraphQLObjectFieldFull : GraphQLObjectField
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
