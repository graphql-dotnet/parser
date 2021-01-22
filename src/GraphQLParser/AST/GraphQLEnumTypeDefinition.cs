using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLEnumTypeDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        public List<GraphQLDirective>? Directives { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.EnumTypeDefinition;

        public List<GraphQLEnumValueDefinition>? Values { get; set; }
    }

    internal sealed class GraphQLEnumTypeDefinitionFull : GraphQLEnumTypeDefinition
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
