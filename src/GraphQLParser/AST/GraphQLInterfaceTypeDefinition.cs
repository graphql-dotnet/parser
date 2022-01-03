namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.InterfaceTypeDefinition"/>.
    /// </summary>
    public class GraphQLInterfaceTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InterfaceTypeDefinition;

        public GraphQLImplementsInterfaces? Interfaces { get; set; }

        /// <inheritdoc/>
        public GraphQLDirectives? Directives { get; set; }

        public GraphQLFieldsDefinition? Fields { get; set; }
    }

    internal sealed class GraphQLInterfaceTypeDefinitionWithLocation : GraphQLInterfaceTypeDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLInterfaceTypeDefinitionWithComment : GraphQLInterfaceTypeDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLInterfaceTypeDefinitionFull : GraphQLInterfaceTypeDefinition
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
