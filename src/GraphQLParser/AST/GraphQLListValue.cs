using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLListValue : GraphQLValue
    {
        private readonly ASTNodeKind _kind;

        public GraphQLListValue(ASTNodeKind kind)
        {
            _kind = kind;
        }

        public ROM AstValue { get; set; }

        public override ASTNodeKind Kind => _kind;

        public List<GraphQLValue>? Values { get; set; }

        /// <inheritdoc/>
        public override string ToString() => AstValue.ToString();
    }
}
