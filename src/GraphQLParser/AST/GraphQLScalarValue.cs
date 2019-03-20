namespace GraphQLParser.AST
{
    public class GraphQLScalarValue : GraphQLValue
    {
        private readonly ASTNodeKind kindField;

        public GraphQLScalarValue(ASTNodeKind kind)
        {
            kindField = kind;
        }

        public override ASTNodeKind Kind => kindField;

        public string Value { get; set; }

        public override string ToString()
        {
            if (Kind == ASTNodeKind.StringValue)
                return $"\"{Value}\"";

            return Value.ToString();
        }
    }
}