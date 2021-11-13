using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.FieldDefinition"/>.
    /// </summary>
    public class GraphQLFieldDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.FieldDefinition;

        public List<GraphQLInputValueDefinition>? Arguments { get; set; }

        public GraphQLType? Type { get; set; }

        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }
    }

    internal sealed class GraphQLFieldDefinitionWithLocation : GraphQLFieldDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLFieldDefinitionWithComment : GraphQLFieldDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLFieldDefinitionFull : GraphQLFieldDefinition
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
