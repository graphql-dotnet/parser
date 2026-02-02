namespace GraphQLParser.AST;

/// <summary>
/// Represents an AST node that may have arguments.
/// </summary>
public interface IHasArgumentsNode
{
    /// <summary>
    /// Arguments of the node represented as a nested node.
    /// </summary>
    public GraphQLArguments? Arguments { get; set; }
}
