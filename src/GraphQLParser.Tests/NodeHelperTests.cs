using System;
using System.Collections.Generic;
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
            var node = method.GetParameters().Length == 1
                ? (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.None })
                : (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.None, Activator.CreateInstance(method.GetParameters()[1].ParameterType) });

            node.Comments = new List<GraphQLComment> { new GraphQLComment("abcdef") };
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
            var node = method.GetParameters().Length == 1
                ? (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.Comments })
                : (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.Comments, Activator.CreateInstance(method.GetParameters()[1].ParameterType) });

            node.Comments = new List<GraphQLComment> { new GraphQLComment("abcdef") };
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
            var node = method.GetParameters().Length == 1
              ? (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.Locations })
              : (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.Locations, Activator.CreateInstance(method.GetParameters()[1].ParameterType) });

            node.Comments = new List<GraphQLComment> { new GraphQLComment("abcdef") };
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
            var node = method.GetParameters().Length == 1
                ? (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.All })
                : (ASTNode)method.Invoke(null, new object[] { IgnoreOptions.All, Activator.CreateInstance(method.GetParameters()[1].ParameterType) });

            node.Comments = new List<GraphQLComment> { new GraphQLComment("abcdef") };
            node.Comment.ShouldBeNull();

            node.Location = new GraphQLLocation(100, 200);
            node.Location.ShouldBe(default);
        }
    }
}
