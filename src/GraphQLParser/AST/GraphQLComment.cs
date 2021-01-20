using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <inheritdoc cref="ASTNodeKind.Comment"/>
    [DebuggerDisplay("{TextString}")]
    public class GraphQLComment : ASTNode
    {
        private ROM _text;
        private string? _textString;

        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Comment;

        /// <summary>
        /// Comment value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Text
        {
            get => _text;
            set
            {
                _text = value;
                _textString = null;
            }
        }

        /// <summary>
        /// Gets comment value represented as string. The value of this property is cached and in sync with <see cref="Text"/>.
        /// The first time this property is accessed, memory in the managed heap will be allocated for it.
        /// In scenarios where minimum memory consumption is required, use the <see cref="Text"/> property.
        /// </summary>
        public string TextString => _textString ??= (string)Text;
    }
}
