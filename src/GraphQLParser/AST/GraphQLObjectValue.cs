using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// AST node for <see cref="ASTNodeKind.ObjectValue"/>.
    /// </summary>
    public class GraphQLObjectValue : GraphQLValue
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ObjectValue;

        public List<GraphQLObjectField>? Fields { get; set; }
    }

    internal sealed class GraphQLObjectValueWithLocation : GraphQLObjectValue
    {
        private GraphQLLocation _location;

        public override GraphQLLocation Location
        {
            get => _location;
            set => _location = value;
        }
    }

    internal sealed class GraphQLObjectValueWithComment : GraphQLObjectValue
    {
        private GraphQLComment? _comment;

        public override GraphQLComment? Comment
        {
            get => _comment;
            set => _comment = value;
        }
    }

    internal sealed class GraphQLObjectValueFull : GraphQLObjectValue
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
