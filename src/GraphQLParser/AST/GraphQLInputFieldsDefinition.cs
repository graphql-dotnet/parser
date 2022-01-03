using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.InputFieldsDefinition"/>.
    /// </summary>
    public class GraphQLInputFieldsDefinition : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.InputFieldsDefinition;

        /// <summary>
        /// List of arguments definitions for this node.
        /// </summary>
        public List<GraphQLInputValueDefinition> Items { get; set; } = null!;
    }

    internal sealed class GraphQLInputFieldsDefinitionWithLocation : GraphQLInputFieldsDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLInputFieldsDefinitionWithComment : GraphQLInputFieldsDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLInputFieldsDefinitionFull : GraphQLInputFieldsDefinition
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
