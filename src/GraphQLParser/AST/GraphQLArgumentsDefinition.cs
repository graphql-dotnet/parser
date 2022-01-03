using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.ArgumentsDefinition"/>.
    /// </summary>
    public class GraphQLArgumentsDefinition : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ArgumentsDefinition;

        /// <summary>
        /// List of arguments definitions for this node.
        /// </summary>
        public List<GraphQLInputValueDefinition> InputValueDefinitions { get; set; } = null!;
    }

    internal sealed class GraphQLArgumentsDefinitionWithLocation : GraphQLArgumentsDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLArgumentsDefinitionWithComment : GraphQLArgumentsDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLArgumentsDefinitionFull : GraphQLArgumentsDefinition
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
