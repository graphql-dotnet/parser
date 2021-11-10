namespace GraphQLParser.AST
{
    public class GraphQLVariableDefinition : ASTNode
    {
        public object? DefaultValue { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.VariableDefinition;

        public GraphQLType? Type { get; set; }

        public GraphQLVariable? Variable { get; set; }
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
