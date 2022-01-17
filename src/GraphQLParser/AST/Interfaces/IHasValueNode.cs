namespace GraphQLParser.AST;

/// <summary>
/// AST node that has its own text.
/// These nodes are:
/// <list type="number">
/// <item><see cref="GraphQLName"/></item>
/// <item><see cref="GraphQLNullValue"/></item>
/// <item><see cref="GraphQLBooleanValue"/></item>
/// <item><see cref="GraphQLIntValue"/></item>
/// <item><see cref="GraphQLFloatValue"/></item>
/// <item><see cref="GraphQLStringValue"/></item>
/// </list>
/// </summary>
public interface IHasValueNode
{
    /// <summary>
    /// Value of AST node represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; set; }
}
