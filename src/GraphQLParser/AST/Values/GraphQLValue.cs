namespace GraphQLParser.AST;

/// <summary>
/// Base AST node for all value nodes.
/// <br/>
/// <see href="http://spec.graphql.org/October2021/#Value"/>
/// </summary>
public abstract class GraphQLValue : ASTNode
{
    /// <summary>
    /// Returns the CLR object that represents the value of this AST node.
    /// </summary>
    public abstract object? ClrValue { get; }
}
