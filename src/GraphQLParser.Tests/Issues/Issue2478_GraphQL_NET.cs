namespace GraphQLParser.Tests;

public class Issue2478_GraphQL_NET
{
    [Theory]
    [InlineData(IgnoreOptions.None)]
    [InlineData(IgnoreOptions.Comments)]
    [InlineData(IgnoreOptions.Locations)]
    [InlineData(IgnoreOptions.All)]
    public void TestSchemaWithTwoInterfaceImplemented(IgnoreOptions options)
    {
        string query = @"
            type Query {
              me: Account
            }

            interface FooInterface {
              id: String
            }

            interface BooInterface {
              name: String
            }

            type Account implements FooInterface & BooInterface {
              id: String
              name: String
            }
        ";

        var document = query.Parse(new ParserOptions { Ignore = options });
        document.Definitions.Count.ShouldBe(4);
        var def = document.Definitions.Last() as GraphQLObjectTypeDefinition;
        def.Interfaces.Count.ShouldBe(2);
    }
}
