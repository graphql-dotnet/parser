namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.InputObjectTypeDefinition"/>.
    /// </summary>
    public class GraphQLInputObjectTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeDefinition;

        /// <inheritdoc/>
        public GraphQLDirectives? Directives { get; set; }

        public GraphQLInputFieldsDefinition? Fields { get; set; }
    }

    internal sealed class GraphQLInputObjectTypeDefinitionWithLocation : GraphQLInputObjectTypeDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLInputObjectTypeDefinitionWithComment : GraphQLInputObjectTypeDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLInputObjectTypeDefinitionFull : GraphQLInputObjectTypeDefinition
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
