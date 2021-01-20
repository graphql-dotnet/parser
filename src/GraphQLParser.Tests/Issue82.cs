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

        [Fact]
        public void Parse_Named_And_Literal_Variables()
        {
            using var document = _query.Parse();

            var def = document.Definitions[0] as GraphQLOperationDefinition;
            def.VariableDefinitions.Count.ShouldBe(1);
            def.VariableDefinitions[0].Type.ShouldBeOfType<GraphQLNamedType>().Name.Value.ShouldBe("String");
            def.VariableDefinitions[0].Variable.Name.Value.ShouldBe("username");

            var selection = def.SelectionSet.Selections[0].ShouldBeOfType<GraphQLFieldSelection>();
            selection.Arguments.Count.ShouldBe(2);
            selection.Arguments[0].Value.ShouldBeOfType<GraphQLVariable>().Name.Value.ShouldBe("username");
            selection.Arguments[1].Value.ShouldBeOfType<GraphQLScalarValue>().Value.ShouldBe("Pete");
        }
    }
}
