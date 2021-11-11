using System;
using PublicApiGenerator;
using Shouldly;
using Xunit;

namespace GraphQLParser.ApiTests
{
    /// <summary>
    /// Tests to verify public API surface.
    /// </summary>
    public class ApiApprovalTests
    {
        [Theory]
        [InlineData(typeof(Lexer))]
        public void Public_Api_Should_Not_Change_Inadvertently(Type type)
        {
            string publicApi = type.Assembly.GeneratePublicApi(new ApiGeneratorOptions
            {
                IncludeAssemblyAttributes = false,
                //WhitelistedNamespacePrefixes = new[] { "Microsoft.Extensions.DependencyInjection" },
                ExcludeAttributes = new[] { "System.Diagnostics.DebuggerDisplayAttribute" }
            });

            // See: https://shouldly.readthedocs.io/en/latest/assertions/shouldMatchApproved.html
            // Note: If the AssemblyName.approved.txt file doesn't match the latest publicApi value,
            // this call will try to launch a diff tool to help you out but that can fail on
            // your machine if a diff tool isn't configured/setup.
            publicApi.ShouldMatchApproved(options => options.WithFilenameGenerator((testMethodInfo, discriminator, fileType, fileExtension) => $"{type.Assembly.GetName().Name}.{fileType}.{fileExtension}"));
        }
    }
}
