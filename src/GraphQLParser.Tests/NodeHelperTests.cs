using System.Linq;
using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

public class NodeHelperTests
{
    [Fact]
    public void NodeHelper_Should_Return_Proper_Nodes_1()
    {
        foreach (var method in typeof(NodeHelper).GetMethods().Where(m => m.IsStatic))
        {
            var node = (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.None });

            node.Comment = new GraphQLComment();
            if (node is not GraphQLComment &&
                node is not GraphQLDocument &&
                node is not GraphQLDescription &&
                node is not GraphQLDirectives)
                node.Comment.ShouldNotBeNull();

            node.Location = new GraphQLLocation(100, 200);
            node.Location.ShouldBe(new GraphQLLocation(100, 200));
        }
    }

    [Fact]
    public void NodeHelper_Should_Return_Proper_Nodes_2()
    {
        foreach (var method in typeof(NodeHelper).GetMethods().Where(m => m.IsStatic))
        {
            var node = (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.Comments });

            node.Comment = new GraphQLComment();
            node.Comment.ShouldBeNull();

            node.Location = new GraphQLLocation(100, 200);
            node.Location.ShouldBe(new GraphQLLocation(100, 200));
        }
    }

    [Fact]
    public void NodeHelper_Should_Return_Proper_Nodes_3()
    {
        foreach (var method in typeof(NodeHelper).GetMethods().Where(m => m.IsStatic))
        {
            var node = (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.Locations });

            node.Comment = new GraphQLComment();
            if (node is not GraphQLComment &&
                node is not GraphQLDocument &&
                node is not GraphQLDescription &&
                node is not GraphQLDirectives)
                node.Comment.ShouldNotBeNull();

            node.Location = new GraphQLLocation(100, 200);
            node.Location.ShouldBe(default);
        }
    }

    [Fact]
    public void NodeHelper_Should_Return_Proper_Nodes_4()
    {
        foreach (var method in typeof(NodeHelper).GetMethods().Where(m => m.IsStatic))
        {
            var node = (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.All });

            node.Comment = new GraphQLComment();
            node.Comment.ShouldBeNull();

            node.Location = new GraphQLLocation(100, 200);
            node.Location.ShouldBe(default);
        }
    }
}
