using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.VariablesDefinition"/>.
    /// </summary>
    public class GraphQLVariablesDefinition : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.VariablesDefinition;

        public List<GraphQLVariableDefinition> Items { get; set; } = null!;
    }

    internal sealed class GraphQLVariablesDefinitionWithLocation : GraphQLVariablesDefinition
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLVariablesDefinitionWithComment : GraphQLVariablesDefinition
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLVariablesDefinitionFull : GraphQLVariablesDefinition
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
