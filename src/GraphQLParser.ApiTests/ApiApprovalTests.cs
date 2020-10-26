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
            var api = typeof(Lexer).Assembly.GeneratePublicApi(new ApiGeneratorOptions
            {
                IncludeAssemblyAttributes = false
            });

            string a = AppDomain.CurrentDomain.BaseDirectory!;
            Console.WriteLine("AAAA " + a);
            var approved = File.ReadAllText(Path.Combine(a, "../../../ApiApprovalTests.Public_Api_Should_Not_Change_Inadvertently.approved.txt"));

            api.ShouldBe(approved);
            Console.WriteLine("OOOOOOOKKKKKKKKKKKK")

            //api.ShouldMatchApproved(opt => opt.);
        }
    }
}
