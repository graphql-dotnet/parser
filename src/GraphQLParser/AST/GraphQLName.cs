using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <inheritdoc cref="ASTNodeKind.Name"/>
    [DebuggerDisplay("{ValueString}")]
    public class GraphQLName : ASTNode
    {
        private ROM _value;
        private string? _valueString;

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Name;

        /// <summary>
        /// Name value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueString = null;
            }
        }

        /// <summary>
        /// Gets name value represented as string. The value of this property is cached and in sync with <see cref="Value"/>.
        /// The first time this property is accessed, memory in the managed heap will be allocated for it.
        /// In scenarios where minimum memory consumption is required, use the <see cref="Value"/> property.
        /// </summary>
        public string ValueString => _valueString ??= (string)Value;
    }
}
