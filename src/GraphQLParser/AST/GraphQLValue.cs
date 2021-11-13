namespace GraphQLParser.AST
{
    /// <summary>
    /// Base AST node for four value nodes:
    /// <br/>
    /// <see cref="GraphQLScalarValue">Scalar</see>
    /// <br/>
    /// <see cref="ASTNodeKind.ListValue">List</see>
    /// <br/>
    /// <see cref="ASTNodeKind.ObjectValue">Object</see>
    /// <br/>
    /// <see cref="ASTNodeKind.Variable">Variable</see>
    /// </summary>
    public abstract class GraphQLValue : ASTNode
    {
    }
}
