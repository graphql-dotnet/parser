using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors;

/// <summary>
/// Prints AST into the provided <see cref="TextWriter"/> as a SDL document.
/// </summary>
/// <typeparam name="TContext">Type of the context object passed into all VisitXXX methods.</typeparam>
public class SDLWriter<TContext> : DefaultNodeVisitor<TContext>
    where TContext : IWriteContext
{
    /// <summary>
    /// Creates visitor with default options.
    /// </summary>
    public SDLWriter()
        : this(new SDLWriterOptions())
    {
    }

    /// <summary>
    /// Creates visitor with the specified options.
    /// </summary>
    /// <param name="options">Visitor options.</param>
    public SDLWriter(SDLWriterOptions options)
    {
        Options = options;
    }

    private SDLWriterOptions Options { get; }

    /// <inheritdoc/>
    public override async ValueTask VisitDocumentAsync(GraphQLDocument document, TContext context)
    {
        await VisitAsync(document.Comment, context).ConfigureAwait(false); // Comment always null

        if (document.Definitions.Count > 0) // Definitions always > 0
        {
            for (int i = 0; i < document.Definitions.Count; ++i)
            {
                await VisitAsync(document.Definitions[i], context).ConfigureAwait(false);

                if (i < document.Definitions.Count - 1)
                {
                    await context.WriteLineAsync().ConfigureAwait(false);
                }
            }
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitCommentAsync(GraphQLComment comment, TContext context)
    {
        if (!Options.WriteComments)
            return;

        if (CommentedNodeShouldBeCloseToPreviousNode(context))
            await context.WriteLineAsync().ConfigureAwait(false);

        // starting # should always be printed in case of empty comment
        await WriteIndentAsync(context).ConfigureAwait(false);
        await context.WriteAsync("#").ConfigureAwait(false);
        bool needStartNewLine = false;

        int length = comment.Value.Span.Length;
        for (int i = 0; i < length; ++i)
        {
            if (needStartNewLine)
            {
                await WriteIndentAsync(context).ConfigureAwait(false);
                await context.WriteAsync("#").ConfigureAwait(false);
                needStartNewLine = false;
            }

            char code = comment.Value.Span[i];
            switch (code)
            {
                case '\r':
                    break;

                case '\n':
                    await context.WriteLineAsync().ConfigureAwait(false);
                    needStartNewLine = true;
                    break;

                default:
                    await context.WriteAsync(comment.Value.Slice(i, 1)/*code*/).ConfigureAwait(false);
                    break;
            }
        }

        await context.WriteLineAsync().ConfigureAwait(false);

        if (CommentedNodeShouldBeCloseToPreviousNode(context))
            await WriteIndentAsync(context).ConfigureAwait(false);

        static bool CommentedNodeShouldBeCloseToPreviousNode(TContext context)
        {
            return TryPeekParent(context, out var node) &&
                node is GraphQLArguments ||
                node is GraphQLObjectField ||
                node is GraphQLName ||
                node is GraphQLUnionMemberTypes;
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDescriptionAsync(GraphQLDescription description, TContext context)
    {
        bool ShouldBeMultilineBlockString()
        {
            bool newLineDetected = false;
            var span = description.Value.Span;
            for (int i = 0; i < span.Length; ++i)
            {
                char code = span[i];

                if (code < 0x0020 && code != 0x0009 && code != 0x000A && code != 0x000D)
                    return false;

                if (code == 0x000A)
                    newLineDetected = true;
            }

            // Note that string value without escape symbols and newline symbols
            // MAY BE represented as a single-line BlockString, for example,
            // """SOME TEXT""", but it was decided to use more brief "SOME TEXT"
            // for such cases. WriteMultilineBlockString method ALWAYS builds
            // BlockString printing """ on new line.
            return newLineDetected;
        }

        async ValueTask WriteMultilineBlockString()
        {
            await WriteIndentAsync(context).ConfigureAwait(false);
            await context.WriteAsync("\"\"\"").ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);

            bool needStartNewLine = true;
            int length = description.Value.Span.Length;
            // http://spec.graphql.org/October2021/#BlockStringCharacter
            for (int i = 0; i < length; ++i)
            {
                if (needStartNewLine)
                {
                    await WriteIndentAsync(context).ConfigureAwait(false);
                    needStartNewLine = false;
                }

                char code = description.Value.Span[i];
                switch (code)
                {
                    case '\r':
                        break;

                    case '\n':
                        await context.WriteLineAsync().ConfigureAwait(false);
                        needStartNewLine = true;
                        break;

                    case '"':
                        if (i < length - 2 && description.Value.Span[i + 1] == '"' && description.Value.Span[i + 2] == '"')
                        {
                            await context.WriteAsync("\\\"").ConfigureAwait(false);
                        }
                        else
                        {
                            await context.WriteAsync(description.Value.Slice(i, 1)/*code*/).ConfigureAwait(false); //TODO: change
                        }
                        break;

                    default:
                        await context.WriteAsync(description.Value.Slice(i, 1)/*code*/).ConfigureAwait(false); //TODO: change
                        break;
                }
            }

            await context.WriteLineAsync().ConfigureAwait(false);
            await WriteIndentAsync(context).ConfigureAwait(false);
            await context.WriteAsync("\"\"\"").ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);
        }

        async ValueTask WriteString()
        {
            await WriteIndentAsync(context).ConfigureAwait(false);
            await WriteEncodedStringAsync(context, description.Value).ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);
        }

        // http://spec.graphql.org/October2021/#StringValue
        if (ShouldBeMultilineBlockString())
            await WriteMultilineBlockString();
        else
            await WriteString();
    }

    /// <inheritdoc/>
    public override async ValueTask VisitNameAsync(GraphQLName name, TContext context)
    {
        await VisitAsync(name.Comment, context).ConfigureAwait(false);
        await context.WriteAsync(name.Value).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFragmentNameAsync(GraphQLFragmentName fragmentName, TContext context)
    {
        await VisitAsync(fragmentName.Comment, context).ConfigureAwait(false);
        await VisitAsync(fragmentName.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFragmentDefinitionAsync(GraphQLFragmentDefinition fragmentDefinition, TContext context)
    {
        await VisitAsync(fragmentDefinition.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("fragment ").ConfigureAwait(false);
        await VisitAsync(fragmentDefinition.FragmentName, context).ConfigureAwait(false);
        await context.WriteAsync(" ").ConfigureAwait(false);
        await VisitAsync(fragmentDefinition.TypeCondition, context).ConfigureAwait(false);
        await VisitAsync(fragmentDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(fragmentDefinition.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFragmentSpreadAsync(GraphQLFragmentSpread fragmentSpread, TContext context)
    {
        await VisitAsync(fragmentSpread.Comment, context).ConfigureAwait(false);

        await WriteIndentAsync(context).ConfigureAwait(false);

        await context.WriteAsync("...").ConfigureAwait(false);
        await VisitAsync(fragmentSpread.FragmentName, context).ConfigureAwait(false);
        await VisitAsync(fragmentSpread.Directives, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInlineFragmentAsync(GraphQLInlineFragment inlineFragment, TContext context)
    {
        await VisitAsync(inlineFragment.Comment, context).ConfigureAwait(false);

        await WriteIndentAsync(context).ConfigureAwait(false);

        await context.WriteAsync("... ").ConfigureAwait(false);
        await VisitAsync(inlineFragment.TypeCondition, context).ConfigureAwait(false);
        await VisitAsync(inlineFragment.Directives, context).ConfigureAwait(false);
        await VisitAsync(inlineFragment.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitTypeConditionAsync(GraphQLTypeCondition typeCondition, TContext context)
    {
        await VisitAsync(typeCondition.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("on ").ConfigureAwait(false);
        await VisitAsync(typeCondition.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitImplementsInterfacesAsync(GraphQLImplementsInterfaces implementsInterfaces, TContext context)
    {
        await VisitAsync(implementsInterfaces.Comment, context).ConfigureAwait(false);
        await context.WriteAsync(" implements ").ConfigureAwait(false);

        for (int i = 0; i < implementsInterfaces.Items.Count; ++i)
        {
            await VisitAsync(implementsInterfaces.Items[i], context).ConfigureAwait(false);
            if (i < implementsInterfaces.Items.Count - 1)
                await context.WriteAsync(" & ").ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitSelectionSetAsync(GraphQLSelectionSet selectionSet, TContext context)
    {
        await VisitAsync(selectionSet.Comment, context).ConfigureAwait(false);

        bool freshLine = selectionSet.Comment != null && Options.WriteComments;
        if (!freshLine && TryPeekParent(context, out var node) && (node is GraphQLOperationDefinition op && op.Name is not null || node is not GraphQLOperationDefinition))
        {
            await context.WriteAsync(" {").ConfigureAwait(false);
        }
        else
        {
            await WriteIndentAsync(context).ConfigureAwait(false);
            await context.WriteAsync("{").ConfigureAwait(false);
        }
        await context.WriteLineAsync().ConfigureAwait(false);

        foreach (var selection in selectionSet.Selections)
            await VisitAsync(selection, context).ConfigureAwait(false);

        await WriteIndentAsync(context).ConfigureAwait(false);
        await context.WriteAsync("}").ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitAliasAsync(GraphQLAlias alias, TContext context)
    {
        await VisitAsync(alias.Comment, context).ConfigureAwait(false);

        await WriteIndentAsync(context).ConfigureAwait(false);

        await VisitAsync(alias.Name, context).ConfigureAwait(false);
        await context.WriteAsync(":").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFieldAsync(GraphQLField field, TContext context)
    {
        await VisitAsync(field.Comment, context).ConfigureAwait(false);
        await VisitAsync(field.Alias, context).ConfigureAwait(false);

        if (field.Alias == null)
            await WriteIndentAsync(context).ConfigureAwait(false);
        else if (field.Name.Comment == null || !Options.WriteComments)
            await context.WriteAsync(" ").ConfigureAwait(false);

        await VisitAsync(field.Name, context).ConfigureAwait(false);
        await VisitAsync(field.Arguments, context).ConfigureAwait(false);
        await VisitAsync(field.Directives, context).ConfigureAwait(false);
        await VisitAsync(field.SelectionSet, context).ConfigureAwait(false);

        if (field.SelectionSet == null)
            await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitOperationDefinitionAsync(GraphQLOperationDefinition operationDefinition, TContext context)
    {
        await VisitAsync(operationDefinition.Comment, context).ConfigureAwait(false);
        if (operationDefinition.Name is not null)
        {
            await context.WriteAsync(GetOperationType(operationDefinition.Operation)).ConfigureAwait(false);
            await context.WriteAsync(" ").ConfigureAwait(false);
            await VisitAsync(operationDefinition.Name, context).ConfigureAwait(false);
        }
        await VisitAsync(operationDefinition.Variables, context).ConfigureAwait(false);
        await VisitAsync(operationDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(operationDefinition.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDirectiveDefinitionAsync(GraphQLDirectiveDefinition directiveDefinition, TContext context)
    {
        await VisitAsync(directiveDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(directiveDefinition.Description, context).ConfigureAwait(false);
        await context.WriteAsync("directive @").ConfigureAwait(false);
        await VisitAsync(directiveDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(directiveDefinition.Arguments, context).ConfigureAwait(false);
        if (directiveDefinition.Repeatable)
            await context.WriteAsync(" repeatable").ConfigureAwait(false);
        await VisitAsync(directiveDefinition.Locations, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDirectiveLocationsAsync(GraphQLDirectiveLocations directiveLocations, TContext context)
    {
        await VisitAsync(directiveLocations.Comment, context).ConfigureAwait(false);
        if (Options.EachDirectiveLocationOnNewLine)
        {
            await context.WriteAsync(" on").ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);
            for (int i = 0; i < directiveLocations.Items.Count; ++i)
            {
                await WriteIndentAsync(context).ConfigureAwait(false);
                await context.WriteAsync("| ").ConfigureAwait(false);
                await context.WriteAsync(GetDirectiveLocation(directiveLocations.Items[i])).ConfigureAwait(false);
                if (i < directiveLocations.Items.Count - 1)
                    await context.WriteLineAsync().ConfigureAwait(false);
            }
        }
        else
        {
            await context.WriteAsync(" on ").ConfigureAwait(false);
            for (int i = 0; i < directiveLocations.Items.Count; ++i)
            {
                await context.WriteAsync(GetDirectiveLocation(directiveLocations.Items[i])).ConfigureAwait(false);
                if (i < directiveLocations.Items.Count - 1)
                    await context.WriteAsync(" | ").ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitVariableDefinitionAsync(GraphQLVariableDefinition variableDefinition, TContext context)
    {
        await VisitAsync(variableDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(variableDefinition.Variable, context).ConfigureAwait(false);
        await context.WriteAsync(": ").ConfigureAwait(false);
        await VisitAsync(variableDefinition.Type, context).ConfigureAwait(false);
        if (variableDefinition.DefaultValue != null)
        {
            await context.WriteAsync(" = ").ConfigureAwait(false);
            await VisitAsync(variableDefinition.DefaultValue, context).ConfigureAwait(false);
        }
        await VisitAsync(variableDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitVariablesDefinitionAsync(GraphQLVariablesDefinition variablesDefinition, TContext context)
    {
        await VisitAsync(variablesDefinition.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("(").ConfigureAwait(false);

        for (int i = 0; i < variablesDefinition.Items.Count; ++i)
        {
            await VisitAsync(variablesDefinition.Items[i], context).ConfigureAwait(false);
            if (i < variablesDefinition.Items.Count - 1)
                await context.WriteAsync(", ").ConfigureAwait(false);
        }

        await context.WriteAsync(")").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitVariableAsync(GraphQLVariable variable, TContext context)
    {
        await VisitAsync(variable.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("$").ConfigureAwait(false);
        await VisitAsync(variable.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitBooleanValueAsync(GraphQLBooleanValue booleanValue, TContext context)
    {
        await VisitAsync(booleanValue.Comment, context).ConfigureAwait(false);
        await context.WriteAsync(booleanValue.Value).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitScalarTypeDefinitionAsync(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context)
    {
        await VisitAsync(scalarTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(scalarTypeDefinition.Description, context).ConfigureAwait(false);
        await context.WriteAsync("scalar ").ConfigureAwait(false);
        await VisitAsync(scalarTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(scalarTypeDefinition.Directives, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitSchemaExtensionAsync(GraphQLSchemaExtension schemaExtension, TContext context)
    {
        await VisitAsync(schemaExtension.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("extend schema").ConfigureAwait(false);
        await VisitAsync(schemaExtension.Directives, context).ConfigureAwait(false);

        //TODO: https://github.com/graphql/graphql-spec/issues/921
        if (schemaExtension.OperationTypes?.Count > 0)
        {
            await context.WriteLineAsync().ConfigureAwait(false);
            await context.WriteAsync("{").ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);

            for (int i = 0; i < schemaExtension.OperationTypes.Count; ++i)
            {
                await VisitAsync(schemaExtension.OperationTypes[i], context).ConfigureAwait(false);
                await context.WriteLineAsync().ConfigureAwait(false);
            }

            await context.WriteAsync("}").ConfigureAwait(false);
        }
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitScalarTypeExtensionAsync(GraphQLScalarTypeExtension scalarTypeExtension, TContext context)
    {
        await VisitAsync(scalarTypeExtension.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("extend scalar ").ConfigureAwait(false);
        await VisitAsync(scalarTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(scalarTypeExtension.Directives, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumTypeDefinitionAsync(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context)
    {
        await VisitAsync(enumTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(enumTypeDefinition.Description, context).ConfigureAwait(false);
        await context.WriteAsync("enum ").ConfigureAwait(false);
        await VisitAsync(enumTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(enumTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(enumTypeDefinition.Values, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumTypeExtensionAsync(GraphQLEnumTypeExtension enumTypeExtension, TContext context)
    {
        await VisitAsync(enumTypeExtension.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("extend enum ").ConfigureAwait(false);
        await VisitAsync(enumTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(enumTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(enumTypeExtension.Values, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumValueDefinitionAsync(GraphQLEnumValueDefinition enumValueDefinition, TContext context)
    {
        await VisitAsync(enumValueDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(enumValueDefinition.Description, context).ConfigureAwait(false);

        await WriteIndentAsync(context).ConfigureAwait(false);

        await VisitAsync(enumValueDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(enumValueDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumValuesDefinitionAsync(GraphQLEnumValuesDefinition enumValuesDefinition, TContext context)
    {
        await VisitAsync(enumValuesDefinition.Comment, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
        await context.WriteAsync("{").ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);

        for (int i = 0; i < enumValuesDefinition.Items.Count; ++i)
        {
            await VisitAsync(enumValuesDefinition.Items[i], context).ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);
        }

        await context.WriteAsync("}").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInputObjectTypeDefinitionAsync(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context)
    {
        await VisitAsync(inputObjectTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeDefinition.Description, context).ConfigureAwait(false);
        await context.WriteAsync("input ").ConfigureAwait(false);
        await VisitAsync(inputObjectTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeDefinition.Fields, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInputObjectTypeExtensionAsync(GraphQLInputObjectTypeExtension inputObjectTypeExtension, TContext context)
    {
        await VisitAsync(inputObjectTypeExtension.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("extend input ").ConfigureAwait(false);
        await VisitAsync(inputObjectTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(inputObjectTypeExtension.Fields, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInputValueDefinitionAsync(GraphQLInputValueDefinition inputValueDefinition, TContext context)
    {
        await VisitAsync(inputValueDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(inputValueDefinition.Description, context).ConfigureAwait(false);

        await WriteIndentAsync(context).ConfigureAwait(false);

        await VisitAsync(inputValueDefinition.Name, context).ConfigureAwait(false);
        await context.WriteAsync(": ").ConfigureAwait(false);
        await VisitAsync(inputValueDefinition.Type, context).ConfigureAwait(false);
        if (inputValueDefinition.DefaultValue != null)
        {
            await context.WriteAsync(" = ").ConfigureAwait(false);
            await VisitAsync(inputValueDefinition.DefaultValue, context).ConfigureAwait(false);
        }
        await VisitAsync(inputValueDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInputFieldsDefinitionAsync(GraphQLInputFieldsDefinition inputFieldsDefinition, TContext context)
    {
        await VisitAsync(inputFieldsDefinition.Comment, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
        await context.WriteAsync("{").ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);

        for (int i = 0; i < inputFieldsDefinition.Items.Count; ++i)
        {
            await VisitAsync(inputFieldsDefinition.Items[i], context).ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);
        }

        await context.WriteAsync("}").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitObjectTypeDefinitionAsync(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context)
    {
        await VisitAsync(objectTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Description, context).ConfigureAwait(false);
        await context.WriteAsync("type ").ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Interfaces, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(objectTypeDefinition.Fields, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitObjectTypeExtensionAsync(GraphQLObjectTypeExtension objectTypeExtension, TContext context)
    {
        await VisitAsync(objectTypeExtension.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("extend type ").ConfigureAwait(false);
        await VisitAsync(objectTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(objectTypeExtension.Interfaces, context).ConfigureAwait(false);
        await VisitAsync(objectTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(objectTypeExtension.Fields, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInterfaceTypeDefinitionAsync(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context)
    {
        await VisitAsync(interfaceTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Description, context).ConfigureAwait(false);
        await context.WriteAsync("interface ").ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Interfaces, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeDefinition.Fields, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInterfaceTypeExtensionAsync(GraphQLInterfaceTypeExtension interfaceTypeExtension, TContext context)
    {
        await VisitAsync(interfaceTypeExtension.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("extend interface ").ConfigureAwait(false);
        await VisitAsync(interfaceTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeExtension.Interfaces, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(interfaceTypeExtension.Fields, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFieldDefinitionAsync(GraphQLFieldDefinition fieldDefinition, TContext context)
    {
        await VisitAsync(fieldDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Description, context).ConfigureAwait(false);

        await WriteIndentAsync(context).ConfigureAwait(false);

        await VisitAsync(fieldDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Arguments, context).ConfigureAwait(false);
        await context.WriteAsync(": ").ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Type, context).ConfigureAwait(false);
        await VisitAsync(fieldDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFieldsDefinitionAsync(GraphQLFieldsDefinition fieldsDefinition, TContext context)
    {
        await VisitAsync(fieldsDefinition.Comment, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
        await context.WriteAsync("{").ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);

        for (int i = 0; i < fieldsDefinition.Items.Count; ++i)
        {
            await VisitAsync(fieldsDefinition.Items[i], context).ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);
        }

        await context.WriteAsync("}").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitSchemaDefinitionAsync(GraphQLSchemaDefinition schemaDefinition, TContext context)
    {
        await VisitAsync(schemaDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(schemaDefinition.Description, context).ConfigureAwait(false);
        await context.WriteAsync("schema").ConfigureAwait(false);
        await VisitAsync(schemaDefinition.Directives, context).ConfigureAwait(false);

        await context.WriteLineAsync().ConfigureAwait(false);
        await context.WriteAsync("{").ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);

        if (schemaDefinition.OperationTypes.Count > 0)
        {
            for (int i = 0; i < schemaDefinition.OperationTypes.Count; ++i)
            {
                await VisitAsync(schemaDefinition.OperationTypes[i], context).ConfigureAwait(false);
                await context.WriteLineAsync().ConfigureAwait(false);
            }
        }

        await context.WriteAsync("}").ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitRootOperationTypeDefinitionAsync(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context)
    {
        await VisitAsync(rootOperationTypeDefinition.Comment, context).ConfigureAwait(false);

        await WriteIndentAsync(context).ConfigureAwait(false);

        await context.WriteAsync(GetOperationType(rootOperationTypeDefinition.Operation)).ConfigureAwait(false);
        await context.WriteAsync(": ").ConfigureAwait(false);
        await VisitAsync(rootOperationTypeDefinition.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitUnionTypeDefinitionAsync(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context)
    {
        await VisitAsync(unionTypeDefinition.Comment, context).ConfigureAwait(false);
        await VisitAsync(unionTypeDefinition.Description, context).ConfigureAwait(false);
        await context.WriteAsync("union ").ConfigureAwait(false);
        await VisitAsync(unionTypeDefinition.Name, context).ConfigureAwait(false);
        await VisitAsync(unionTypeDefinition.Directives, context).ConfigureAwait(false);
        await VisitAsync(unionTypeDefinition.Types, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitUnionTypeExtensionAsync(GraphQLUnionTypeExtension unionTypeExtension, TContext context)
    {
        await VisitAsync(unionTypeExtension.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("extend union ").ConfigureAwait(false);
        await VisitAsync(unionTypeExtension.Name, context).ConfigureAwait(false);
        await VisitAsync(unionTypeExtension.Directives, context).ConfigureAwait(false);
        await VisitAsync(unionTypeExtension.Types, context).ConfigureAwait(false);
        await context.WriteLineAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitUnionMemberTypesAsync(GraphQLUnionMemberTypes unionMemberTypes, TContext context)
    {
        await VisitAsync(unionMemberTypes.Comment, context).ConfigureAwait(false);

        if (unionMemberTypes.Comment == null || !Options.WriteComments)
            await context.WriteAsync(" ").ConfigureAwait(false);

        if (Options.EachUnionMemberOnNewLine)
        {
            await context.WriteAsync("=").ConfigureAwait(false);
            await context.WriteLineAsync().ConfigureAwait(false);

            for (int i = 0; i < unionMemberTypes.Items.Count; ++i)
            {
                await WriteIndentAsync(context).ConfigureAwait(false);
                await context.WriteAsync("| ").ConfigureAwait(false);
                await VisitAsync(unionMemberTypes.Items[i], context).ConfigureAwait(false);
                if (i < unionMemberTypes.Items.Count - 1)
                    await context.WriteLineAsync().ConfigureAwait(false);
            }
        }
        else
        {
            await context.WriteAsync("= ").ConfigureAwait(false);

            for (int i = 0; i < unionMemberTypes.Items.Count; ++i)
            {
                await VisitAsync(unionMemberTypes.Items[i], context).ConfigureAwait(false);
                if (i < unionMemberTypes.Items.Count - 1)
                    await context.WriteAsync(" | ").ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDirectiveAsync(GraphQLDirective directive, TContext context)
    {
        await VisitAsync(directive.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("@").ConfigureAwait(false);
        await VisitAsync(directive.Name, context).ConfigureAwait(false);
        await VisitAsync(directive.Arguments, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDirectivesAsync(GraphQLDirectives directives, TContext context)
    {
        await VisitAsync(directives.Comment, context).ConfigureAwait(false); // Comment always null - see ParserContext.ParseDirectives
        await context.WriteAsync(" ").ConfigureAwait(false);

        for (int i = 0; i < directives.Items.Count; ++i)
        {
            await VisitAsync(directives.Items[i], context).ConfigureAwait(false);
            if (i < directives.Items.Count - 1)
                await context.WriteAsync(" ").ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitArgumentAsync(GraphQLArgument argument, TContext context)
    {
        await VisitAsync(argument.Comment, context).ConfigureAwait(false);
        await VisitAsync(argument.Name, context).ConfigureAwait(false);
        await context.WriteAsync(": ").ConfigureAwait(false);
        await VisitAsync(argument.Value, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitArgumentsDefinitionAsync(GraphQLArgumentsDefinition argumentsDefinition, TContext context)
    {
        await VisitAsync(argumentsDefinition.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("(").ConfigureAwait(false);

        for (int i = 0; i < argumentsDefinition.Items.Count; ++i)
        {
            await VisitAsync(argumentsDefinition.Items[i], context).ConfigureAwait(false);
            if (i < argumentsDefinition.Items.Count - 1)
                await context.WriteAsync(", ").ConfigureAwait(false);
        }

        await context.WriteAsync(")").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitArgumentsAsync(GraphQLArguments arguments, TContext context)
    {
        await VisitAsync(arguments.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("(").ConfigureAwait(false);
        for (int i = 0; i < arguments.Items.Count; ++i)
        {
            await VisitAsync(arguments.Items[i], context).ConfigureAwait(false);
            if (i < arguments.Items.Count - 1)
                await context.WriteAsync(", ").ConfigureAwait(false);
        }
        await context.WriteAsync(")").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitNonNullTypeAsync(GraphQLNonNullType nonNullType, TContext context)
    {
        await VisitAsync(nonNullType.Comment, context).ConfigureAwait(false);
        await VisitAsync(nonNullType.Type, context).ConfigureAwait(false);
        await context.WriteAsync("!").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitListTypeAsync(GraphQLListType listType, TContext context)
    {
        await VisitAsync(listType.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("[").ConfigureAwait(false);
        await VisitAsync(listType.Type, context).ConfigureAwait(false);
        await context.WriteAsync("]").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitListValueAsync(GraphQLListValue listValue, TContext context)
    {
        await VisitAsync(listValue.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("[").ConfigureAwait(false);
        if (listValue.Values?.Count > 0)
        {
            for (int i = 0; i < listValue.Values.Count; ++i)
            {
                await VisitAsync(listValue.Values[i], context).ConfigureAwait(false);
                if (i < listValue.Values.Count - 1)
                    await context.WriteAsync(", ").ConfigureAwait(false);
            }
        }
        await context.WriteAsync("]").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitNullValueAsync(GraphQLNullValue nullValue, TContext context)
    {
        await VisitAsync(nullValue.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("null").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitStringValueAsync(GraphQLStringValue stringValue, TContext context)
    {
        await VisitAsync(stringValue.Comment, context).ConfigureAwait(false);
        await WriteEncodedStringAsync(context, stringValue.Value);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitIntValueAsync(GraphQLIntValue intValue, TContext context)
    {
        await VisitAsync(intValue.Comment, context).ConfigureAwait(false);
        await context.WriteAsync(intValue.Value).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFloatValueAsync(GraphQLFloatValue floatValue, TContext context)
    {
        await VisitAsync(floatValue.Comment, context).ConfigureAwait(false);
        await context.WriteAsync(floatValue.Value).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumValueAsync(GraphQLEnumValue enumValue, TContext context)
    {
        await VisitAsync(enumValue.Comment, context).ConfigureAwait(false);
        await VisitAsync(enumValue.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitObjectValueAsync(GraphQLObjectValue objectValue, TContext context)
    {
        await VisitAsync(objectValue.Comment, context).ConfigureAwait(false);
        await context.WriteAsync("{").ConfigureAwait(false);
        if (objectValue.Fields?.Count > 0)
        {
            for (int i = 0; i < objectValue.Fields.Count; ++i)
            {
                await VisitAsync(objectValue.Fields[i], context).ConfigureAwait(false);
                if (i < objectValue.Fields.Count - 1)
                    await context.WriteAsync(", ").ConfigureAwait(false);
            }
        }
        await context.WriteAsync("}").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitObjectFieldAsync(GraphQLObjectField objectField, TContext context)
    {
        await VisitAsync(objectField.Comment, context).ConfigureAwait(false);
        await VisitAsync(objectField.Name, context).ConfigureAwait(false);
        await context.WriteAsync(": ").ConfigureAwait(false);
        await VisitAsync(objectField.Value, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override ValueTask VisitNamedTypeAsync(GraphQLNamedType namedType, TContext context)
    {
        return base.VisitNamedTypeAsync(namedType, context);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitAsync(ASTNode? node, TContext context)
    {
        if (node == null)
            return;

        int prevLevel = context.IndentLevel;

        if (node is GraphQLField ||
            node is GraphQLFieldDefinition ||
            node is GraphQLInputFieldsDefinition ||
            node is GraphQLEnumValueDefinition ||
            node is GraphQLFragmentSpread ||
            node is GraphQLInlineFragment ||
            node is GraphQLRootOperationTypeDefinition)
            ++context.IndentLevel;

        if (node is GraphQLDirectiveLocations && Options.EachDirectiveLocationOnNewLine)
            ++context.IndentLevel;

        if (node is GraphQLUnionMemberTypes && Options.EachUnionMemberOnNewLine)
            ++context.IndentLevel;

        context.Parents.Push(node);

        await base.VisitAsync(node, context).ConfigureAwait(false);

        context.Parents.Pop();
        context.IndentLevel = prevLevel;
    }

    private static string GetOperationType(OperationType type) => type switch
    {
        OperationType.Query => "query",
        OperationType.Mutation => "mutation",
        OperationType.Subscription => "subscription",

        _ => throw new NotSupportedException($"Unknown operation {type}"),
    };

    private static string GetDirectiveLocation(DirectiveLocation location) => location switch
    {
        // http://spec.graphql.org/October2021/#ExecutableDirectiveLocation
        DirectiveLocation.Query => "QUERY",
        DirectiveLocation.Mutation => "MUTATION",
        DirectiveLocation.Subscription => "SUBSCRIPTION",
        DirectiveLocation.Field => "FIELD",
        DirectiveLocation.FragmentDefinition => "FRAGMENT_DEFINITION",
        DirectiveLocation.FragmentSpread => "FRAGMENT_SPREAD",
        DirectiveLocation.InlineFragment => "INLINE_FRAGMENT",
        DirectiveLocation.VariableDefinition => "VARIABLE_DEFINITION",

        // http://spec.graphql.org/October2021/#TypeSystemDirectiveLocation
        DirectiveLocation.Schema => "SCHEMA",
        DirectiveLocation.Scalar => "SCALAR",
        DirectiveLocation.Object => "OBJECT",
        DirectiveLocation.FieldDefinition => "FIELD_DEFINITION",
        DirectiveLocation.ArgumentDefinition => "ARGUMENT_DEFINITION",
        DirectiveLocation.Interface => "INTERFACE",
        DirectiveLocation.Union => "UNION",
        DirectiveLocation.Enum => "ENUM",
        DirectiveLocation.EnumValue => "ENUM_VALUE",
        DirectiveLocation.InputObject => "INPUT_OBJECT",
        DirectiveLocation.InputFieldDefinition => "INPUT_FIELD_DEFINITION",

        _ => throw new NotSupportedException($"Unknown directive location {location}"),
    };

    private static async ValueTask WriteIndentAsync(TContext context)
    {
        for (int i = 0; i < context.IndentLevel; ++i)
            await context.WriteAsync("  ").ConfigureAwait(false);
    }

    private static bool TryPeekParent(TContext context, [NotNullWhen(true)] out ASTNode? node)
    {
        node = null;

        using var e = context.Parents.GetEnumerator();
        for (int i = 0; i < 2; ++i)
            if (!e.MoveNext())
                return false;

        node = e.Current;
        return true;
    }

    // http://spec.graphql.org/October2021/#StringCharacter
    private static async ValueTask WriteEncodedStringAsync(TContext context, ROM value)
    {
        await context.WriteAsync("\"").ConfigureAwait(false);

        for (int i = 0; i < value.Span.Length; ++i)
        {
            char code = value.Span[i];
            if (code < ' ')
            {
                if (code == '\b')
                    await context.WriteAsync("\\b").ConfigureAwait(false);
                else if (code == '\f')
                    await context.WriteAsync("\\f").ConfigureAwait(false);
                else if (code == '\n')
                    await context.WriteAsync("\\n").ConfigureAwait(false);
                else if (code == '\r')
                    await context.WriteAsync("\\r").ConfigureAwait(false);
                else if (code == '\t')
                    await context.WriteAsync("\\t").ConfigureAwait(false);
                else
                    await context.WriteAsync("\\u" + ((int)code).ToString("X4")).ConfigureAwait(false);
            }
            else if (code == '\\')
                await context.WriteAsync("\\\\").ConfigureAwait(false);
            else if (code == '"')
                await context.WriteAsync("\\\"").ConfigureAwait(false);
            else
                await context.WriteAsync(value.Slice(i, 1)/*code*/).ConfigureAwait(false); // TODO: no method for char
        }

        await context.WriteAsync("\"").ConfigureAwait(false);
    }
}

/// <summary>
/// Options for <see cref="SDLWriter{TContext}"/>.
/// </summary>
public class SDLWriterOptions
{
    /// <summary>
    /// Write comments into the output.
    /// </summary>
    public bool WriteComments { get; set; }

    /// <summary>
    /// Whether to write each directive location on its own line.
    /// </summary>
    public bool EachDirectiveLocationOnNewLine { get; set; }

    /// <summary>
    /// Whether to write each union member on its own line.
    /// </summary>
    public bool EachUnionMemberOnNewLine { get; set; }
}

