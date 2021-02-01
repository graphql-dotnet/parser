namespace GraphQLParser.AST
{
    public class GraphQLNamedType : GraphQLType, INamedNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.NamedType;

        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }

        /// <inheritdoc/>
        public override string ToString() => Name?.Value.ToString()!;
    }

    internal sealed class GraphQLNamedTypeFull : GraphQLNamedType
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
