using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.ObjectTypeExtension"/>.
    /// </summary>
    public class GraphQLObjectTypeExtension : GraphQLTypeExtension, IHasDirectivesNode, IHasInterfacesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ObjectTypeExtension;

        public List<GraphQLNamedType>? Interfaces { get; set; }

        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        public List<GraphQLFieldDefinition>? Fields { get; set; }
    }

    internal sealed class GraphQLObjectTypeExtensionWithLocation : GraphQLObjectTypeExtension
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLObjectTypeExtensionWithComment : GraphQLObjectTypeExtension
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLObjectTypeExtensionFull : GraphQLObjectTypeExtension
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
