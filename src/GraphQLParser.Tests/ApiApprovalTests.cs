using PublicApiGenerator;
using Shouldly;
using Xunit;

namespace GraphQLParser.Tests
{
    public class ApiApprovalTests
    {
        [Fact]
        public void Public_Api_Should_Not_Change_Inadvertently()
        {
            string publicApi = ApiGenerator.GeneratePublicApi(typeof(Lexer).Assembly, shouldIncludeAssemblyAttributes: false);

            publicApi.ShouldMatchApproved();
        }
    }
}
