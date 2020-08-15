using Shouldly;
using Xunit;

namespace GraphQLParser.Tests
{
    public class SourceTests
    {
        [Fact]
        public void CreateSourceFromString_BodyEqualsToProvidedSource()
        {
            var source = new Source("somesrc");

            source.Body.ShouldBe("somesrc");
        }

        [Fact]
        public void CreateSourceFromString_SourceNameEqualsToGraphQL()
        {
            var source = new Source("somesrc");

            source.Name.ShouldBe("GraphQL");
        }

        [Fact]
        public void CreateSourceFromStringWithName_SourceNameEqualsToProvidedName()
        {
            var source = new Source("somesrc", "somename");

            source.Name.ShouldBe("somename");
        }
    }
}
