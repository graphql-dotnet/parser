namespace GraphQLParser.AST;

/// <summary>
/// Represents an AST node that may have default value.
/// Now these nodes are <see cref="GraphQLInputValueDefinition"/> and <see cref="GraphQLVariableDefinition"/>.
/// </summary>
public interface IHasDefaultValueNode
{
    /// <summary>
    /// Nested <see cref="GraphQLValue"/> AST node with default value (if any).
    /// </summary>
    public GraphQLValue? DefaultValue { get; set; }
}
