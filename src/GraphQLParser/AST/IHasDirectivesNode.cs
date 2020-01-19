using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public interface IHasDirectivesNode
    {
        List<GraphQLDirective> Directives { get; set; }
    }
}
