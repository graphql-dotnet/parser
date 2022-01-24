using System;
using System.Threading;
using System.Threading.Tasks;
using GraphQLParser.AST;
using GraphQLParser.Visitors;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests.Visitors;

public class DefaultNodeVisitorTests
{
    private class Context : IASTVisitorContext
    {
        public CancellationToken CancellationToken => default;
    }

    private class MySuperNode : ASTNode
    {
        public override ASTNodeKind Kind => (ASTNodeKind)12345;
    }

    [Fact]
    public void DefaultNodeVisitor_Should_Handle_Null()
    {
        var visitor = new ASTVisitor<Context>();
        var context = new Context();
        visitor.VisitAsync(null, context).ShouldBe(new ValueTask());
    }

    [Fact]
    public void DefaultNodeVisitor_Should_Throw_On_Unknown_Node()
    {
        var visitor = new ASTVisitor<Context>();
        var context = new Context();

        var ex = Should.Throw<NotSupportedException>(() => visitor.VisitAsync(new MySuperNode(), context));
        ex.Message.ShouldBe("Unknown node 'MySuperNode'.");
    }
}
