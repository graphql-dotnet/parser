using GraphQLParser.Visitors;

namespace GraphQLParser.Tests.Visitors;

public class ASTVisitorTests
{
    private class Context : IASTVisitorContext
    {
        public CancellationToken CancellationToken { get; set; }
    }

    private class MySuperNode : ASTNode
    {
        public override ASTNodeKind Kind => (ASTNodeKind)12345;
    }

    [Fact]
    public void ASTVisitor_Should_Handle_Null()
    {
        var visitor = new ASTVisitor<Context>();
        var context = new Context();
        visitor.VisitAsync(null, context).ShouldBe(default);
    }

    [Fact]
    public async Task ASTVisitor_Should_Throw_On_Unknown_Node()
    {
        var visitor = new ASTVisitor<Context>();
        var context = new Context();

        var ex = await Should.ThrowAsync<NotSupportedException>(async () => await visitor.VisitAsync(new MySuperNode(), context));
        ex.Message.ShouldBe("Unknown node 'MySuperNode'.");
    }

    [Fact]
    public async Task ASTVisitor_Should_Pass_CancellationToken()
    {
        var document = "scalar JSON".Parse();
        var visitor = new MyVisitor();
        using var cts = new CancellationTokenSource(500);
        var context = new Context { CancellationToken = cts.Token };
        context.CancellationToken.ThrowIfCancellationRequested();

        await Should.ThrowAsync<OperationCanceledException>(async () => await visitor.VisitAsync(document, context));
    }

    private class MyVisitor : ASTVisitor<Context>
    {
        protected override async ValueTask VisitScalarTypeDefinitionAsync(GraphQLScalarTypeDefinition scalarTypeDefinition, Context context)
        {
            await Task.Delay(700);
            context.CancellationToken.ThrowIfCancellationRequested();
            await base.VisitScalarTypeDefinitionAsync(scalarTypeDefinition, context);
        }
    }
}
