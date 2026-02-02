namespace GraphQLParser.AST;

/// <summary>
/// Represents an AST node that may have directives applied to it (type, argument, field, enum, etc.).
/// </summary>
public interface IHasDirectivesNode
{
    /// <summary>
    /// Directives of the AST node represented as a nested node.
    /// </summary>
    public GraphQLDirectives? Directives { get; set; }
}
