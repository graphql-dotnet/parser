using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <inheritdoc cref="ASTNodeKind.Name"/>
    [DebuggerDisplay("{Value}")]
    public class GraphQLName : ASTNode
    {
        /// <inheritdoc/>
        public override ASTNodeKind Kind => ASTNodeKind.Name;

        /// <summary>
        /// Name value represented as <see cref="ROM"/>.
        /// </summary>
        public ROM Value { get; set; }
    }
}
