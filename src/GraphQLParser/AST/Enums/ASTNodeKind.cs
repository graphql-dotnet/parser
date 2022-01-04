namespace GraphQLParser.AST
{
    /// <summary>
    /// The kind of nodes in the GraphQL document AST (Abstract Syntax Tree).
    /// </summary>
    public enum ASTNodeKind
    {
        /// <summary>
        /// Named thing inside GraphQL document: operations, fields, arguments,
        /// types, directives, fragments, and variables.
        /// <br/>
        /// All names must follow the same grammatical form: [_A-Za-z][_0-9A-Za-z]*
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Name"/>
        /// </summary>
        Name,

        /// <summary>
        /// A GraphQL Document describes a complete file or request string
        /// operated on by a GraphQL service or client. A document contains
        /// multiple definitions, either executable or representative of a
        /// GraphQL type system.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Document"/>
        /// </summary>
        Document,

        /// <summary>
        /// Each operation is represented by an optional operation name and a selection set.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#OperationDefinition"/>
        /// </summary>
        OperationDefinition,

        /// <summary>
        /// A GraphQL operation can be parameterized with variables, maximizing reuse,
        /// and avoiding costly string building in clients at runtime.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#VariableDefinition"/>
        /// </summary>
        VariableDefinition,

        /// <summary>
        /// If not defined as constant (for example, in DefaultValue),
        /// a Variable can be supplied for an input value.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Variable"/>
        /// </summary>
        Variable,

        /// <summary>
        /// An operation selects the set of information it needs, and will
        /// receive exactly that information and nothing more, avoiding
        /// over-fetching and under-fetching data.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#SelectionSet"/>
        /// </summary>
        SelectionSet,

        /// <summary>
        /// A selection set is primarily composed of fields. A field describes
        /// one discrete piece of information available to request within a selection set.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Field"/>
        /// </summary>
        Field,

        /// <summary>
        /// Fields are conceptually functions which return values, and occasionally
        /// accept arguments which alter their behavior.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Argument"/>
        /// </summary>
        Argument,

        /// <summary>
        /// Fragments are the primary unit of composition in GraphQL.
        /// Fragments allow for the reuse of common repeated selections of fields,
        /// reducing duplicated text in the document.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#FragmentSpread"/>
        /// </summary>
        FragmentSpread,

        /// <summary>
        /// Fragments can be defined inline within a selection set.
        /// This is done to conditionally include fields based on their runtime type.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#InlineFragment"/>
        /// </summary>
        InlineFragment,

        /// <summary>
        /// Fragments are the primary unit of composition in GraphQL.
        /// Fragments allow for the reuse of common repeated selections of fields,
        /// reducing duplicated text in the document.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#FragmentDefinition"/>
        /// </summary>
        FragmentDefinition,

        /// <summary>
        /// An integer number is specified without a decimal point or exponent (ex. 1).
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#IntValue"/>
        /// </summary>
        IntValue,

        /// <summary>
        /// A Float number includes either a decimal point (ex. 1.0) or
        /// an exponent (ex. 1e50) or both (ex. 6.0221413e23).
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#FloatValue"/>
        /// </summary>
        FloatValue,

        /// <summary>
        /// Strings are sequences of characters wrapped in double‐quotes ("). (ex. "Hello World").
        /// White space and other otherwise‐ignored characters are significant within a string value.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#StringValue"/>
        /// </summary>
        StringValue,

        /// <summary>
        /// Boolean value. The two keywords true and false represent the two boolean values.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#BooleanValue"/>
        /// </summary>
        BooleanValue,

        /// <summary>
        /// Enum values are represented as unquoted names (ex. MOBILE_WEB). It is recommended
        /// that Enum values be "all caps". Enum values are only used in contexts where the
        /// precise enumeration type is known. Therefore it’s not necessary to supply an
        /// enumeration type name in the literal.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#EnumValue"/>
        /// </summary>
        EnumValue,

        /// <summary>
        /// Lists are ordered sequences of values wrapped in square-brackets [ ].
        /// The values of a List literal may be any value literal or variable (ex. [1, 2, 3]).
        /// Commas are optional throughout GraphQL so trailing commas are allowed and repeated
        /// commas do not represent missing values.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ListValue"/>
        /// </summary>
        ListValue,

        /// <summary>
        /// Input object literal values are unordered lists of keyed input values wrapped
        /// in curly-braces { }. The values of an object literal may be any input value
        /// literal or variable (ex. { name: "Hello world", score: 1.0 }).
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ObjectValue"/>
        /// </summary>
        ObjectValue,

        /// <summary>
        /// A keyed input value within <see cref="ObjectValue"/>.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ObjectField"/>
        /// </summary>
        ObjectField,

        /// <summary>
        /// Applied directive. Directives provide a way to describe alternate runtime execution
        /// and type validation behavior in a GraphQL document. Directives can be used to describe
        /// additional information for types, fields, fragments, operations, etc.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Directive"/>
        /// </summary>
        Directive,

        /// <summary>
        /// The fundamental unit of any GraphQL Schema is the type. There are six kinds of named
        /// type definitions in GraphQL, and two wrapping types. In other words all non-wrapping
        /// types are referred to as "named types".
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#NamedType"/>
        /// </summary>
        NamedType,

        /// <summary>
        /// Wrapping type. A GraphQL schema may describe that a field represents a list of
        /// another type; the List type is provided for this reason, and wraps another type.
        /// A wrapping type has an underlying named type, found by continually unwrapping
        /// the type until a named type is found.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ListType"/>
        /// </summary>
        ListType,

        /// <summary>
        /// Wrapping type. The Non-Null type wraps another type, and denotes that the resulting
        /// value will never be null (and that a field error cannot result in a null value).
        /// A wrapping type has an underlying named type, found by continually unwrapping
        /// the type until a named type is found.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#NonNullType"/>
        /// </summary>
        NonNullType,

        /// <summary>
        /// Null values are represented as the keyword null.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#NullValue"/>
        /// </summary>
        NullValue,

        /// <summary>
        /// A GraphQL service’s collective type system capabilities
        /// are referred to as that service's "schema".
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#SchemaDefinition"/>
        /// </summary>
        SchemaDefinition,

        /// <summary>
        /// Root operation type for each kind of operation: query, mutation, and subscription;
        /// this determines the place in the type system where those operations begin.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#RootOperationTypeDefinition"/>
        /// </summary>
        RootOperationTypeDefinition,

        /// <summary>
        /// Scalar types represent primitive leaf values in a GraphQL type system.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ScalarTypeDefinition"/>
        /// </summary>
        ScalarTypeDefinition,

        /// <summary>
        /// While Scalar types describe the leaf values of these hierarchical
        /// operations, Objects describe the intermediate levels.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ObjectTypeDefinition"/>
        /// </summary>
        ObjectTypeDefinition,

        /// <summary>
        /// GraphQL Objects represent a list of named fields,
        /// each of which yield a value of a specific type.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#FieldDefinition"/>
        /// </summary>
        FieldDefinition,

        /// <summary>
        /// Argument name and its expected input type.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#InputValueDefinition"/>
        /// </summary>
        InputValueDefinition,

        /// <summary>
        /// GraphQL interfaces represent a list of named fields and their arguments.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#InterfaceTypeDefinition"/>
        /// </summary>
        InterfaceTypeDefinition,

        /// <summary>
        /// GraphQL Unions represent an object that could be one of a list of GraphQL
        /// Object types, but provides for no guaranteed fields between those types.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#UnionTypeDefinition"/>
        /// </summary>
        UnionTypeDefinition,

        /// <summary>
        /// GraphQL Enum types, like Scalar types, also represent leaf values in
        /// a GraphQL type system. However Enum types describe the set of possible values.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#EnumTypeDefinition"/>
        /// </summary>
        EnumTypeDefinition,

        /// <summary>
        /// GraphQL Enum types, like Scalar types, also represent leaf values in
        /// a GraphQL type system. However Enum types describe the set of possible values.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#EnumValueDefinition"/>
        /// </summary>
        EnumValueDefinition,

        /// <summary>
        /// A GraphQL Input Object defines a set of input fields; the input fields
        /// are either scalars, enums, or other input objects. This allows arguments
        /// to accept arbitrarily complex structs.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#InputObjectTypeDefinition"/>
        /// </summary>
        InputObjectTypeDefinition,

        /// <summary>
        /// Object type extensions are used to represent a type which has been
        /// extended from some original type. For example, this might be used
        /// to represent local data, or by a GraphQL service which is itself
        /// an extension of another GraphQL service.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ObjectTypeExtension"/>
        /// </summary>
        ObjectTypeExtension,

        /// <summary>
        /// Directive definition. Directives provide a way to describe alternate runtime execution
        /// and type validation behavior in a GraphQL document. Directives can be used to describe
        /// additional information for types, fields, fragments, operations, etc.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#DirectiveDefinition"/>
        /// </summary>
        DirectiveDefinition,

        /// <summary>
        /// GraphQL source documents may contain single‐line comments, starting with the # marker.
        /// A comment can contain any Unicode code point except LineTerminator so a comment always
        /// consists of all code points starting with the # character up to but not including the line terminator.
        /// Comments behave like white space and may appear after any token, or before a line terminator,
        /// and have no significance to the semantic meaning of a GraphQL Document.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Comment"/>
        /// </summary>
        Comment,

        /// <summary>
        /// Description of a GraphQL definition.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Description"/>
        /// </summary>
        Description,

        /// <summary>
        /// Fragments must specify the type they apply to.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#TypeCondition"/>
        /// </summary>
        TypeCondition,

        /// <summary>
        /// By default a field's response key in the response object will use that
        /// field's name. However, you can define a different response key by
        /// specifying an alias.Fragments must specify the type they apply to.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Alias"/>
        /// </summary>
        Alias,

        /// <summary>
        /// Scalar type extensions are used to represent a scalar type which has been
        /// extended from some original scalar type. For example, this might be used by
        /// a GraphQL tool or service which adds directives to an existing scalar.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ScalarTypeExtension"/>
        /// </summary>
        ScalarTypeExtension,

        /// <summary>
        /// Interface type extensions are used to represent an interface which has been
        /// extended from some original interface. For example, this might be used
        /// to represent common local data on many types, or by a GraphQL service which is itself
        /// an extension of another GraphQL service.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#InterfaceTypeExtension"/>
        /// </summary>
        InterfaceTypeExtension,

        /// <summary>
        /// Union type extensions are used to represent a union type which has been
        /// extended from some original union type. For example, this might be used
        /// to represent additional local data, or by a GraphQL service which is itself
        /// an extension of another GraphQL service.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#UnionTypeExtension"/>
        /// </summary>
        UnionTypeExtension,

        /// <summary>
        /// Enum type extensions are used to represent an enum type which has been
        /// extended from some original enum type. For example, this might be used
        /// to represent additional local data, or by a GraphQL service which is itself
        /// an extension of another GraphQL service.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#EnumTypeExtension"/>
        /// </summary>
        EnumTypeExtension,

        /// <summary>
        /// Input object type extensions are used to represent an input object type which has been
        /// extended from some original input object type. For example, this might be used
        /// by a GraphQL service which is itself an extension of another GraphQL service.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#InputObjectTypeExtension"/>
        /// </summary>
        InputObjectTypeExtension,

        /// <summary>
        /// Occasionally object fields/directives can accept arguments to further specify
        /// the return value. Object field arguments are defined as a list of all possible
        /// argument names and their expected input types.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ArgumentsDefinition"/>
        /// </summary>
        ArgumentsDefinition,

        /// <summary>
        /// Fields are conceptually functions which return values, and occasionally
        /// accept arguments which alter their behavior.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Arguments"/>
        /// <br/>
        /// This node represents a list of <see cref="Argument"/>.
        /// </summary>
        Arguments,

        /// <summary>
        /// A GraphQL Input Object defines a set of input fields; the input fields
        /// are either scalars, enums, or other input objects. This allows arguments
        /// to accept arbitrarily complex structs.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#InputFieldsDefinition"/>
        /// <br/>
        /// This node represents a list of <see cref="InputValueDefinition"/>.
        /// </summary>
        InputFieldsDefinition,

        /// <summary>
        /// A GraphQL operation can be parameterized with variables, maximizing reuse,
        /// and avoiding costly string building in clients at runtime.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#VariableDefinitions"/>
        /// <br/>
        /// This node represents a list of <see cref="VariableDefinition"/>.
        /// </summary>
        VariablesDefinition, // TODO: https://github.com/graphql/graphql-spec/issues/915

        /// <summary>
        /// GraphQL Enum types, like Scalar types, also represent leaf values in
        /// a GraphQL type system. However Enum types describe the set of possible values.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#EnumValuesDefinition"/>
        /// <br/>
        /// This node represents a list of <see cref="EnumValueDefinition"/>.
        /// </summary>
        EnumValuesDefinition,

        /// <summary>
        /// GraphQL Objects represent a list of named fields,
        /// each of which yield a value of a specific type.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#FieldsDefinition"/>
        /// <br/>
        /// This node represents a list of <see cref="FieldDefinition"/>.
        /// </summary>
        FieldsDefinition,

        /// <summary>
        /// Applied directive. Directives provide a way to describe alternate runtime execution
        /// and type validation behavior in a GraphQL document. Directives can be used to describe
        /// additional information for types, fields, fragments, operations, etc.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#Directives"/>
        /// <br/>
        /// This node represents a list of <see cref="Directive"/>.
        /// </summary>
        Directives,

        /// <summary>
        /// A list of implemented interfaces.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#ImplementsInterfaces"/>
        /// <br/>
        /// This node represents a list of <see cref="NamedType"/>.
        /// </summary>
        ImplementsInterfaces,

        /// <summary>
        /// A list of directive locations.
        /// <br/>
        /// <see href="http://spec.graphql.org/October2021/#DirectiveLocations"/>
        /// <br/>
        /// This node represents a list of <see cref="DirectiveLocation"/>.
        /// </summary>
        DirectiveLocations,
    }
}
