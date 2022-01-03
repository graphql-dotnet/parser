namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.VariableDefinition"/>.
    /// </summary>
    public class GraphQLVariableDefinition : ASTNode, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.VariableDefinition;

        public GraphQLVariable? Variable { get; set; }

        public GraphQLType? Type { get; set; }

        public GraphQLValue? DefaultValue { get; set; }

        /// <inheritdoc/>
        public GraphQLDirectives? Directives { get; set; }
    }

    internal sealed class GraphQLVariableDefinitionWithLocation : GraphQLVariableDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLVariableDefinitionWithComment : GraphQLVariableDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }
    internal sealed class GraphQLVariableDefinitionFull : GraphQLVariableDefinition
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
