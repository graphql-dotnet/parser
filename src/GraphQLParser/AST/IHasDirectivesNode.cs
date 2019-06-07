using System.Collections.Generic;

namespace GraphQLParser.AST
{
    public interface IHasDirectivesNode
    {
        IEnumerable<GraphQLDirective> Directives { get; set; }
    }
}
