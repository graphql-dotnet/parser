using System;
using System.Diagnostics;

namespace GraphQLParser.AST
{
    [DebuggerDisplay("{Value}")]
    public class GraphQLName : ASTNode
    {
        public override ASTNodeKind Kind => ASTNodeKind.Name;

        public ReadOnlyMemory<char> Value { get; set; }
    }
}
