using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.ScalarTypeExtension"/>.
    /// </summary>
    public class GraphQLScalarTypeExtension : GraphQLTypeExtension, IHasDirectivesNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ScalarTypeExtension;

        /// <inheritdoc/>
        public List<GraphQLDirective>? Directives { get; set; }
    }

    internal sealed class GraphQLScalarTypeExtensionWithLocation : GraphQLScalarTypeExtension
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLScalarTypeExtensionWithComment : GraphQLScalarTypeExtension
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLScalarTypeExtensionFull : GraphQLScalarTypeExtension
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
