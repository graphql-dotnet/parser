using System.Diagnostics;

namespace GraphQLParser.AST
{
    [DebuggerDisplay("{Value}")]
    public class GraphQLName : ASTNode
    {
        private ROM _value;
        private string? _valueString;

        public override ASTNodeKind Kind => ASTNodeKind.Name;

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
    }
}
