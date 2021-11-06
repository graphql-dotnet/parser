using System.Diagnostics;

namespace GraphQLParser.AST
{
    /// <inheritdoc cref="ASTNodeKind.Description"/>
    [DebuggerDisplay("{Value}")]
    public class GraphQLDescription : ASTNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.Description;

        public ROM Value { get; set; }
    }
}
