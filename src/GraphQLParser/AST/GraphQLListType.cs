namespace GraphQLParser.AST
{
    public class GraphQLListType : GraphQLType
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ListType;

        public GraphQLType? Type { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"[{Type}]";
    }

    internal sealed class GraphQLListTypeFull : GraphQLListType
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
