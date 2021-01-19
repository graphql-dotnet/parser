namespace GraphQLParser.AST
{
    public class GraphQLScalarValue : GraphQLValue
    {
        private readonly ASTNodeKind _kind;
        private ROM _value;
        private string? _valueString;

        public GraphQLScalarValue(ASTNodeKind kind)
        {
            _kind = kind;
        }

        public override ASTNodeKind Kind => _kind;

        public ROM Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueString = null;
            }
        }

        public string ValueString => _valueString ??= (string)Value;

        public override string? ToString() => Kind == ASTNodeKind.StringValue ? $"\"{Value}\"" : Value.ToString();
    }
}
