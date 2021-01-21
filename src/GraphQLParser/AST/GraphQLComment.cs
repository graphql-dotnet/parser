using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <inheritdoc cref="ASTNodeKind.Comment"/>
    [DebuggerDisplay("{Text}")]
    public class GraphQLComment : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Comment;

        /// <summary>
        /// Comment value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Text { get; set; }
    }
}
