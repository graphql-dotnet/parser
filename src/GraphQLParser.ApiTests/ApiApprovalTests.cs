using System;
using System.IO;
using System.Reflection;
using PublicApiGenerator;
using Shouldly;
using Xunit;

namespace GraphQLParser.ApiTests
{
    public class ApiApprovalTests
    {
        [Fact]
        public void Public_Api_Should_Not_Change_Inadvertently()
        {
            typeof(Lexer).Assembly.GeneratePublicApi(new ApiGeneratorOptions
            {
                IncludeAssemblyAttributes = false
            }).ShouldMatchApproved();
        }
    }
}
