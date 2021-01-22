namespace GraphQLParser.AST
{
    public class GraphQLTypeExtensionDefinition : GraphQLTypeDefinition
    {
        public GraphQLObjectTypeDefinition? Definition { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.TypeExtensionDefinition;
    }

    internal sealed class GraphQLTypeExtensionDefinitionFull : GraphQLTypeExtensionDefinition
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
