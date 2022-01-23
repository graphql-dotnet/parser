using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests;

// https://github.com/graphql-dotnet/parser/issues/127
// https://github.com/graphql/graphql-spec/pull/598
public class Issue127
{
    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void Should_Parse_Single_Interface(IgnoreOptions options)
    {
        // https://github.com/graphql/graphql-spec/pull/598#issuecomment-815439917
        string query = @"interface Foo

{ alpha: beta }";
        var document = query.Parse(new ParserOptions { Ignore = options });
        document.ShouldNotBeNull();
        document.Definitions.Count.ShouldBe(1);
        var def = document.Definitions[0].ShouldBeAssignableTo<GraphQLInterfaceTypeDefinition>();
        def.Fields.Count.ShouldBe(1);
        def.Fields[0].Name.Value.ShouldBe("alpha");
        def.Fields[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("beta");
    }
}
