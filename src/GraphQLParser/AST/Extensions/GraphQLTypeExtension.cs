namespace GraphQLParser.AST;

/// <summary>
/// Base AST node for six extension nodes:
/// <br/>
/// <see cref="ASTNodeKind.ScalarTypeExtension">ScalarTypeExtension</see>
/// <br/>
/// <see cref="ASTNodeKind.ObjectTypeExtension">ObjectTypeExtension</see>
/// <br/>
/// <see cref="ASTNodeKind.InterfaceTypeExtension">InterfaceTypeExtension</see>
/// <br/>
/// <see cref="ASTNodeKind.UnionTypeExtension">UnionTypeExtension</see>
/// <br/>
/// <see cref="ASTNodeKind.EnumTypeExtension">EnumTypeExtension</see>
/// <br/>
/// <see cref="ASTNodeKind.InputObjectTypeExtension">InputObjectTypeExtension</see>
/// </summary>
public abstract class GraphQLTypeExtension : ASTNode, INamedNode
{
    /// <summary>Initializes a new instance.</summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLTypeExtension()
    {
        Name = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLTypeExtension"/>.
    /// </summary>
    protected GraphQLTypeExtension(GraphQLName name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public GraphQLName Name { get; set; }
}
