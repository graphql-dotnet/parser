namespace GraphQLParser.AST
{
    /// <summary>
    /// Represents an AST node that may have description applied to it.
    /// As a rule, these are definition nodes: type, arguments, enum values etc.
    /// </summary>
    public interface IHasDescriptionNode
    {
        /// <summary>
        /// Description of a GraphQL definition.
        /// </summary>
        GraphQLDescription? Description { get; set; }
    }
}
