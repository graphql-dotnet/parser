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

        /// <summary>
        /// An operation selects the set of information it needs, and will
        /// receive exactly that information and nothing more, avoiding
        /// over-fetching and under-fetching data.
        /// </summary>
        SelectionSet,

        /// <summary>
        /// A selection set is primarily composed of fields. A field describes
        /// one discrete piece of information available to request within a selection set.
        /// </summary>
        Field,

        /// <summary>
        /// Fields are conceptually functions which return values, and occasionally
        /// accept arguments which alter their behavior.
        /// </summary>
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

        /// <summary>
        /// Enum values are represented as unquoted names (ex. MOBILE_WEB). It is recommended
        /// that Enum values be "all caps". Enum values are only used in contexts where the
        /// precise enumeration type is known. Therefore it’s not necessary to supply an
        /// enumeration type name in the literal.
        /// </summary>
        EnumValue,

        /// <summary>
        /// Lists are ordered sequences of values wrapped in square-brackets [ ].
        /// The values of a List literal may be any value literal or variable (ex. [1, 2, 3]).
        /// Commas are optional throughout GraphQL so trailing commas are allowed and repeated
        /// commas do not represent missing values.
        /// </summary>
        ListValue,

        /// <summary>
        /// Input object literal values are unordered lists of keyed input values wrapped
        /// in curly-braces { }. The values of an object literal may be any input value
        /// literal or variable (ex. { name: "Hello world", score: 1.0 }).
        /// </summary>
        ObjectValue,

        /// <summary>
        /// A keyed input value within <see cref="ObjectValue"/>.
        /// </summary>
        ObjectField,

        /// <summary>
        /// Applied directive. Directives provide a way to describe alternate runtime execution
        /// and type validation behavior in a GraphQL document. Directives can be used to describe
        /// additional information for types, fields, fragments, operations, etc.
        /// </summary>
        Directive,

        /// <summary>
        /// The fundamental unit of any GraphQL Schema is the type. There are six kinds of named
        /// type definitions in GraphQL, and two wrapping types. In other words all non-wrapping
        /// types are referred to as "named types".
        /// </summary>
        NamedType,

        /// <summary>
        /// Wrapping type. A GraphQL schema may describe that a field represents a list of
        /// another type; the List type is provided for this reason, and wraps another type.
        /// A wrapping type has an underlying named type, found by continually unwrapping
        /// the type until a named type is found.
        /// </summary>
        ListType,

        /// <summary>
        /// Wrapping type. The Non-Null type wraps another type, and denotes that the resulting
        /// value will never be null (and that a field error cannot result in a null value).
        /// A wrapping type has an underlying named type, found by continually unwrapping
        /// the type until a named type is found.
        /// </summary>
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

        /// <summary>
        /// Description of a GraphQL definition.
        /// </summary>
        Description,
    }
}
