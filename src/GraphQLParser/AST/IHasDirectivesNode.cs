using System.Collections.Generic;

namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may have directives applied to it (type, argument, field, enum, etc.).
    /// </summary>
    public interface IHasDirectivesNode
    {
        /// <summary>
        /// Directives of the node represented as a list of nested nodes.
        /// </summary>
        List<GraphQLDirective>? Directives { get; set; }
    }
}
