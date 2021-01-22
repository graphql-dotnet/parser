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

    internal sealed class GraphQLNonNullTypeFull : GraphQLNonNullType
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
