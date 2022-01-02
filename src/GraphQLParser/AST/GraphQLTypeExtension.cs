namespace GraphQLParser.AST
{
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
        /// <inheritdoc/>
        public GraphQLName? Name { get; set; }
    }
}
