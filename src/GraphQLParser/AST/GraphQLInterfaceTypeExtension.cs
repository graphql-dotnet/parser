namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.InterfaceTypeExtension"/>.
    /// </summary>
    public class GraphQLInterfaceTypeExtension : GraphQLTypeExtension, IHasDirectivesNode, IHasInterfacesNode, IHasFieldsDefinitionNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InterfaceTypeExtension;

        public GraphQLImplementsInterfaces? Interfaces { get; set; }

        /// <inheritdoc/>
        public GraphQLDirectives? Directives { get; set; }

        public GraphQLFieldsDefinition? Fields { get; set; }
    }

    internal sealed class GraphQLInterfaceTypeExtensionWithLocation : GraphQLInterfaceTypeExtension
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLInterfaceTypeExtensionWithComment : GraphQLInterfaceTypeExtension
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLInterfaceTypeExtensionFull : GraphQLInterfaceTypeExtension
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
