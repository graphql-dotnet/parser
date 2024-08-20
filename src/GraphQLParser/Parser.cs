using GraphQLParser.AST;
using GraphQLParser.Exceptions;

namespace GraphQLParser;

/// <summary>
/// Parser for GraphQL syntax.
/// </summary>
public static class Parser
{
    /// <summary>
    /// Generates AST based on input text.
    /// </summary>
    /// <param name="source">Input data as a sequence of characters.</param>
    /// <param name="options">Parser options.</param>
    /// <returns>AST (Abstract Syntax Tree) for GraphQL document.</returns>
    /// <exception cref="GraphQLSyntaxErrorException">In case when parser recursion depth exceeds <see cref="ParserOptions.MaxDepth"/>.</exception>
    /// <exception cref="GraphQLMaxDepthExceededException">In case of syntax error.</exception>
    public static GraphQLDocument Parse(ROM source, ParserOptions options = default)
        => Parse<GraphQLDocument>(source, options);

    /// <summary>
    /// Generates AST based on input text.
    /// </summary>
    /// <typeparam name="T">Type of node to parse input text as.</typeparam>
    /// <param name="source">Input data as a sequence of characters.</param>
    /// <param name="options">Parser options.</param>
    /// <returns>AST (Abstract Syntax Tree) for GraphQL node.</returns>
    /// <exception cref="GraphQLSyntaxErrorException">In case when parser recursion depth exceeds <see cref="ParserOptions.MaxDepth"/>.</exception>
    /// <exception cref="GraphQLMaxDepthExceededException">In case of syntax error.</exception>
    /// <exception cref="NotSupportedException">The specified node type is unsupported.</exception>
    public static T Parse<T>(ROM source, ParserOptions options = default)
        where T : ASTNode
    {
        var context = new ParserContext(source, options);
        T result;

        // main use case
        if (typeof(T) == typeof(GraphQLDocument))
            result = (T)(object)context.ParseDocument();

        else if (typeof(T) == typeof(GraphQLValue))
            result = (T)(object)context.ParseValueLiteral(false);
        else if (typeof(T) == typeof(GraphQLVariable))
            result = (T)(object)context.ParseVariable();
        else if (typeof(T) == typeof(GraphQLArgument))
            result = (T)(object)context.ParseArgument();
        else if (typeof(T) == typeof(GraphQLArguments))
            result = (T)(object)context.ParseArguments();
        else if (typeof(T) == typeof(GraphQLDescription))
            result = (T)(object)context.ParseDescription();
        else if (typeof(T) == typeof(GraphQLDirective))
            result = (T)(object)context.ParseDirective();
        else if (typeof(T) == typeof(GraphQLDirectives))
            result = (T)(object)context.ParseDirectives();
        else if (typeof(T) == typeof(GraphQLField))
            result = (T)(object)context.ParseField();
        else if (typeof(T) == typeof(GraphQLSelectionSet))
            result = (T)(object)context.ParseSelectionSet();

        // definitions
        else if (typeof(T) == typeof(GraphQLArgumentsDefinition))
            result = (T)(object)context.ParseArgumentsDefinition();
        else if (typeof(T) == typeof(GraphQLDirectiveDefinition))
            result = (T)(object)context.ParseDirectiveDefinition();
        else if (typeof(T) == typeof(GraphQLEnumTypeDefinition))
            result = (T)(object)context.ParseEnumTypeDefinition();
        else if (typeof(T) == typeof(GraphQLEnumValueDefinition))
            result = (T)(object)context.ParseEnumValueDefinition();
        else if (typeof(T) == typeof(GraphQLEnumValuesDefinition))
            result = (T)(object)context.ParseEnumValuesDefinition();
        else if (typeof(T) == typeof(GraphQLFieldDefinition))
            result = (T)(object)context.ParseFieldDefinition();
        else if (typeof(T) == typeof(GraphQLFieldsDefinition))
            result = (T)(object)context.ParseFieldsDefinition();
        else if (typeof(T) == typeof(GraphQLFragmentDefinition))
            result = (T)(object)context.ParseFragmentDefinition();
        else if (typeof(T) == typeof(GraphQLInputFieldsDefinition))
            result = (T)(object)context.ParseInputFieldsDefinition();
        else if (typeof(T) == typeof(GraphQLInputObjectTypeDefinition))
            result = (T)(object)context.ParseInputObjectTypeDefinition();
        else if (typeof(T) == typeof(GraphQLInputValueDefinition))
            result = (T)(object)context.ParseInputValueDefinition(null);
        else if (typeof(T) == typeof(GraphQLInputFieldDefinition))
            result = (T)(object)context.ParseInputValueDefinition(false);
        else if (typeof(T) == typeof(GraphQLArgumentDefinition))
            result = (T)(object)context.ParseInputValueDefinition(true);
        else if (typeof(T) == typeof(GraphQLInterfaceTypeDefinition))
            result = (T)(object)context.ParseInterfaceTypeDefinition();
        else if (typeof(T) == typeof(GraphQLObjectTypeDefinition))
            result = (T)(object)context.ParseObjectTypeDefinition();
        else if (typeof(T) == typeof(GraphQLOperationDefinition))
            result = (T)(object)context.ParseOperationDefinition();
        else if (typeof(T) == typeof(GraphQLRootOperationTypeDefinition))
            result = (T)(object)context.ParseRootOperationTypeDefinition();
        else if (typeof(T) == typeof(GraphQLScalarTypeDefinition))
            result = (T)(object)context.ParseScalarTypeDefinition();
        else if (typeof(T) == typeof(GraphQLSchemaDefinition))
            result = (T)(object)context.ParseSchemaDefinition();
        else if (typeof(T) == typeof(GraphQLUnionTypeDefinition))
            result = (T)(object)context.ParseUnionTypeDefinition();
        else if (typeof(T) == typeof(GraphQLVariableDefinition))
            result = (T)(object)context.ParseVariableDefinition();
        else if (typeof(T) == typeof(GraphQLVariablesDefinition))
            result = (T)(object)context.ParseVariablesDefinition();

        // extensions
        else if (typeof(T) == typeof(GraphQLSchemaExtension) || typeof(GraphQLTypeExtension).IsAssignableFrom(typeof(T)))
            result = (T)(object)context.ParseTypeSystemExtension();

        else
            throw new NotSupportedException();

        context.Expect(TokenKind.EOF);

        return result;
    }
}
