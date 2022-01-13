namespace GraphQLParser.AST;

/// <summary>
/// Represents a node that can be a part of selection set.
/// <br/>
/// Available nodes:
/// <list type="number">
/// <item><see cref="GraphQLField"/></item>
/// <item><see cref="GraphQLFragmentSpread"/></item>
/// <item><see cref="GraphQLInlineFragment"/></item>
/// </list>
/// </summary>
public interface ISelectionNode
{
}
