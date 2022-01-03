namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.InputObjectTypeExtension"/>.
    /// </summary>
    public class GraphQLInputObjectTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InputObjectTypeExtension;

        /// <inheritdoc/>
        public GraphQLDirectives? Directives { get; set; }

        public GraphQLInputFieldsDefinition? Fields { get; set; }
    }

    internal sealed class GraphQLInputObjectTypeExtensionWithLocation : GraphQLInputObjectTypeExtension
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLInputObjectTypeExtensionWithComment : GraphQLInputObjectTypeExtension
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLInputObjectTypeExtensionFull : GraphQLInputObjectTypeExtension
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
