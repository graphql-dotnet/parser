using GraphQLParser.AST;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests
{
    public class Issue82
    {
        private readonly string _query = @"query($username: String) {
  Person(uname: $username, firstName: ""Pete"") {
    id, email
    }
}
";

        [Theory]
        [InlineData(IgnoreOptions.None)]
        [InlineData(IgnoreOptions.IgnoreComments)]
        [InlineData(IgnoreOptions.IgnoreCommentsAndLocations)]
        public void Parse_Named_And_Literal_Variables(IgnoreOptions options)
        {
            using var document = _query.Parse(new ParserOptions { Ignore = options });

            var def = document.Definitions[0] as GraphQLOperationDefinition;
            def.VariableDefinitions.Count.ShouldBe(1);
            def.VariableDefinitions[0].Type.ShouldBeAssignableTo<GraphQLNamedType>().Name.Value.ShouldBe("String");
            def.VariableDefinitions[0].Variable.Name.Value.ShouldBe("username");

            var selection = def.SelectionSet.Selections[0].ShouldBeAssignableTo<GraphQLFieldSelection>();
            selection.Arguments.Count.ShouldBe(2);
            selection.Arguments[0].Value.ShouldBeAssignableTo<GraphQLVariable>().Name.Value.ShouldBe("username");
            selection.Arguments[1].Value.ShouldBeAssignableTo<GraphQLScalarValue>().Value.ShouldBe("Pete");
        }
    }
}
