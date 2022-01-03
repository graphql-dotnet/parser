namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.InputValueDefinition"/>.
    /// </summary>
    public class GraphQLInputValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InputValueDefinition;

        public GraphQLType? Type { get; set; }

        public GraphQLValue? DefaultValue { get; set; }

        /// <inheritdoc/>
        public GraphQLDirectives? Directives { get; set; }
    }

    internal sealed class GraphQLInputValueDefinitionWithLocation : GraphQLInputValueDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLInputValueDefinitionWithComment : GraphQLInputValueDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLInputValueDefinitionFull : GraphQLInputValueDefinition
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
