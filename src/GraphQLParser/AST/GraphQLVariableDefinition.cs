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
