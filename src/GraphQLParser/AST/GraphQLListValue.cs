namespace GraphQLParser.AST
{
    using System.Collections.Generic;

    public class GraphQLListValue : GraphQLValue
    {
        private ASTNodeKind kindField;

        public GraphQLListValue(ASTNodeKind kind)
        {
            kindField = kind;
        }

        public string AstValue { get; set; }

        public override ASTNodeKind Kind => kindField;

        public IEnumerable<GraphQLValue> Values { get; set; }

        public override string ToString() => AstValue;
    }
}