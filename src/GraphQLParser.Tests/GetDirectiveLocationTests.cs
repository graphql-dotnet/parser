using GraphQLParser.Visitors;

namespace GraphQLParser.Tests;

public class GetDirectiveLocationTests
{
    [Fact]
    public async Task ItWorks()
    {
        var sdl = $$"""
            schema @test(value: "{{DirectiveLocation.Schema}}") {
                query: Query
            }

            type Query @test(value: "{{DirectiveLocation.Object}}") {
                field(arg:String @test(value:"{{DirectiveLocation.ArgumentDefinition}}")): String @test(value: "{{DirectiveLocation.FieldDefinition}}")
            }

            scalar CustomScalar @test(value: "{{DirectiveLocation.Scalar}}")

            interface CustomInterface @test(value: "{{DirectiveLocation.Interface}}") {
                field: String @test(value: "{{DirectiveLocation.FieldDefinition}}")
            }

            union CustomUnion @test(value: "{{DirectiveLocation.Union}}") = A | B

            enum CustomEnum @test(value: "{{DirectiveLocation.Enum}}") {
                A @test(value: "{{DirectiveLocation.EnumValue}}")
            }

            input CustomInput @test(value: "{{DirectiveLocation.InputObject}}") {
                field: String @test(value: "{{DirectiveLocation.InputFieldDefinition}}")
            }

            query Query @test(value: "{{DirectiveLocation.Query}}") {
                field @test(value: "{{DirectiveLocation.Field}}")
                ...fragment1 @test(value: "{{DirectiveLocation.FragmentSpread}}")
                ... on CustomType @test(value: "{{DirectiveLocation.InlineFragment}}") {
                    field @test(value: "{{DirectiveLocation.Field}}")
                }
            }

            fragment fragment1 on CustomType @test(value: "{{DirectiveLocation.FragmentDefinition}}") {
                field @test(value: "{{DirectiveLocation.Field}}")
            }

            mutation($arg: String @test(value: "{{DirectiveLocation.VariableDefinition}}")) @test(value: "{{DirectiveLocation.Mutation}}") {
                field @test(value: "{{DirectiveLocation.Field}}")
            }

            subscription @test(value: "{{DirectiveLocation.Subscription}}") {
                field @test(value: "{{DirectiveLocation.Field}}")
            }
            """;
        var ast = Parser.Parse(sdl);
        var context = new MyVisitor.MyContext();
        await new MyVisitor().VisitAsync(ast, context);
        context.Count.ShouldBe(24);
    }

    private sealed class MyVisitor : ASTVisitor<MyVisitor.MyContext>
    {
        public override ValueTask VisitAsync(ASTNode node, MyContext context)
        {
            if (node is IHasDirectivesNode directivesNode)
            {
                var d = directivesNode.Directives?.FirstOrDefault(x => x.Name == "test");
                if (d != null)
                {
                    var arg = d?.Arguments?.FirstOrDefault(x => x.Name == "value")?.Value;
                    var argValue = (arg as GraphQLStringValue)?.Value;
                    var location = node.GetDirectiveLocation();
                    location.ToString().ShouldBe(argValue?.ToString());
                    context.Count++;
                }
            }
            return base.VisitAsync(node, context);
        }

        internal sealed class MyContext : IASTVisitorContext
        {
            public int Count { get; set; }
            public CancellationToken CancellationToken => default;
        }
    }
}
