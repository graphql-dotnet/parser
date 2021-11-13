namespace GraphQLParser.AST
{
    //TODO: change
    public class GraphQLTypeExtensionDefinition : GraphQLTypeDefinition
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.TypeExtensionDefinition;

        public GraphQLObjectTypeDefinition? Definition { get; set; }
    }

    internal sealed class GraphQLTypeExtensionDefinitionWithLocation : GraphQLTypeExtensionDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLTypeExtensionDefinitionWithComment : GraphQLTypeExtensionDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
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
