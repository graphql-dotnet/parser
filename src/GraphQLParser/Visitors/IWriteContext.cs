using System.Collections.Generic;
using System.IO;

namespace GraphQLParser.Visitors
{
    public interface IWriteContext : INodeVisitorContext
    {
        TextWriter Writer { get; }

        Stack<AST.ASTNode> Parent { get; }
    }
}
