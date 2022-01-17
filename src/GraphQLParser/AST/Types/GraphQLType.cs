namespace GraphQLParser.AST;

/// <summary>
/// Base AST node for three type nodes:
/// <br/>
/// <see cref="ASTNodeKind.NamedType">Named</see>
/// <br/>
/// <see cref="ASTNodeKind.NonNullType">NonNull</see>
/// <br/>
/// <see cref="ASTNodeKind.ListType">List</see>
/// </summary>
public abstract class GraphQLType : ASTNode
{
}
