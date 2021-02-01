namespace GraphQLParser.AST
{
    /// <summary>
    /// The kind of nodes in the GraphQL document AST (Abstract Syntax Tree).
    /// </summary>
    public enum ASTNodeKind
    {
        /// <summary>
        /// Named thing inside GraphQL document: operations, fields, arguments, types, directives, fragments, and variables.
        /// <br/>
        /// All names must follow the same grammatical form: [_A-Za-z][_0-9A-Za-z]*
        /// </summary>
        Name,
        Document,
        OperationDefinition,
        VariableDefinition,
        Variable,
        SelectionSet,
        Field,
        Argument,
        FragmentSpread,
        InlineFragment,
        FragmentDefinition,

        /// <summary>
        /// An integer number is specified without a decimal point or exponent (ex. 1).
        /// </summary>
        IntValue,

        /// <summary>
        /// A Float number includes either a decimal point (ex. 1.0) or an exponent (ex. 1e50) or both (ex. 6.0221413e23).
        /// </summary>
        FloatValue,

        /// <summary>
        /// Strings are sequences of characters wrapped in double‐quotes ("). (ex. "Hello World").
        /// White space and other otherwise‐ignored characters are significant within a string value.
        /// </summary>
        StringValue,

        /// <summary>
        /// Boolean value. The two keywords true and false represent the two boolean values.
        /// </summary>
        BooleanValue,
        EnumValue,
        ListValue,
        ObjectValue,
        ObjectField,

        /// <summary>
        /// Applied directive. Directives provide a way to describe alternate runtime execution
        /// and type validation behavior in a GraphQL document. Directives can be used to describe
        /// additional information for types, fields, fragments, operations, etc.
        /// </summary>
        Directive,
        NamedType,
        ListType,
        NonNullType,

        /// <summary>
        /// Null values are represented as the keyword null.
        /// </summary>
        NullValue,
        SchemaDefinition,
        OperationTypeDefinition,
        ScalarTypeDefinition,
        ObjectTypeDefinition,
        FieldDefinition,
        InputValueDefinition,
        InterfaceTypeDefinition,
        UnionTypeDefinition,
        EnumTypeDefinition,
        EnumValueDefinition,
        InputObjectTypeDefinition,
        TypeExtensionDefinition,

        /// <summary>
        /// Directive definition. Directives provide a way to describe alternate runtime execution
        /// and type validation behavior in a GraphQL document. Directives can be used to describe
        /// additional information for types, fields, fragments, operations, etc.
        /// </summary>
        DirectiveDefinition,

        /// <summary>
        /// GraphQL source documents may contain single‐line comments, starting with the # marker.
        /// A comment can contain any Unicode code point except LineTerminator so a comment always
        /// consists of all code points starting with the # character up to but not including the line terminator.
        /// Comments behave like white space and may appear after any token, or before a line terminator,
        /// and have no significance to the semantic meaning of a GraphQL Document.
        /// </summary>
        Comment,
    }
}
