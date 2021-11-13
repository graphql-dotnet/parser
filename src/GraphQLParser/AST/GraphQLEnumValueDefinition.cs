using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.EnumValueDefinition"/>.
    /// </summary>
    public class GraphQLEnumValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.EnumValueDefinition;

        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }
    }

    internal sealed class GraphQLEnumValueDefinitionWithLocation : GraphQLEnumValueDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLEnumValueDefinitionWithComment : GraphQLEnumValueDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLEnumValueDefinitionFull : GraphQLEnumValueDefinition
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
