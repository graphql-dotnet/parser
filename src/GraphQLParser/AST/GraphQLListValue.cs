using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public class GraphQLListValue : GraphQLValue
    {
        private readonly ASTNodeKind _kindField;

        public GraphQLListValue(ASTNodeKind kind)
        {
            _kindField = kind;
        }

        public string? AstValue { get; set; }

        public override ASTNodeKind Kind => _kindField;

        public List<GraphQLValue>? Values { get; set; }

        public override string ToString() => AstValue!;
    }
}
