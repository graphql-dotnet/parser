namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.FieldsDefinition"/>.
    /// </summary>
    public class GraphQLFieldsDefinition : ASTListNode<GraphQLFieldDefinition>
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.FieldsDefinition;
    }

    internal sealed class GraphQLFieldsDefinitionWithLocation : GraphQLFieldsDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLFieldsDefinitionWithComment : GraphQLFieldsDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLFieldsDefinitionFull : GraphQLFieldsDefinition
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
