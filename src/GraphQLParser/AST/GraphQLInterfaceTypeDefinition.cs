using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLInterfaceTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }

        public List<GraphQLFieldDefinition>? Fields { get; set; }

        public List<GraphQLNamedType>? Interfaces { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InterfaceTypeDefinition;
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
