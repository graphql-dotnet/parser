using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLDirectiveDefinition : GraphQLTypeDefinition
    {
        public List<GraphQLInputValueDefinition>? Arguments { get; set; }

        public List<GraphQLInputValueDefinition>? Definitions { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.DirectiveDefinition;

        public List<GraphQLName>? Locations { get; set; }

        public bool Repeatable { get; set; }
    }

    internal sealed class GraphQLDirectiveDefinitionFull : GraphQLDirectiveDefinition
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
