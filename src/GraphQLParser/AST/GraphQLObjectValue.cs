using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLObjectValue : GraphQLValue
    {
        public List<GraphQLObjectField>? Fields { get; set; }

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.ObjectValue;
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
