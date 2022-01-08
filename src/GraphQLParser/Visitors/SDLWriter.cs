using System;
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
    /// <inheritdoc/>
    public override async ValueTask VisitDocument(GraphQLDocument document, TContext context)
    {
        if (document.Definitions.Count > 0)
        {
            for (int i = 0; i < document.Definitions.Count; ++i)
            {
                await Visit(document.Definitions[i], context).ConfigureAwait(false);

                if (i < document.Definitions.Count - 1)
                {
                    await context.WriteLine().ConfigureAwait(false);
                }
            }
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitComment(GraphQLComment comment, TContext context)
    {
        int level = GetLevel(context);

        bool needStartNewLine = true;
        int length = comment.Text.Span.Length;
        for (int i = 0; i < length; ++i)
        {
            if (needStartNewLine)
            {
                await WriteIndent(context, level).ConfigureAwait(false);
                await context.Write("#").ConfigureAwait(false);
                needStartNewLine = false;
            }

            char code = comment.Text.Span[i];
            switch (code)
            {
                case '\r':
                    break;

                case '\n':
                    await context.WriteLine().ConfigureAwait(false);
                    needStartNewLine = true;
                    break;

                default:
                    await context.Write(comment.Text.Slice(i, 1)/*code*/).ConfigureAwait(false);
                    break;
            }
        }

        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDescription(GraphQLDescription description, TContext context)
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
            int level = GetLevel(context);

            await WriteIndent(context, level).ConfigureAwait(false);
            await context.Write("\"\"\"").ConfigureAwait(false);
            await context.WriteLine().ConfigureAwait(false);

            bool needStartNewLine = true;
            int length = description.Value.Span.Length;
            // http://spec.graphql.org/October2021/#BlockStringCharacter
            for (int i = 0; i < length; ++i)
            {
                if (needStartNewLine)
                {
                    await WriteIndent(context, level).ConfigureAwait(false);
                    needStartNewLine = false;
                }

                char code = description.Value.Span[i];
                switch (code)
                {
                    case '\r':
                        break;

                    case '\n':
                        await context.WriteLine().ConfigureAwait(false);
                        needStartNewLine = true;
                        break;

                    case '"':
                        if (i < length - 2 && description.Value.Span[i + 1] == '"' && description.Value.Span[i + 2] == '"')
                        {
                            await context.Write("\\\"").ConfigureAwait(false);
                        }
                        else
                        {
                            await context.Write(description.Value.Slice(i, 1)/*code*/).ConfigureAwait(false); //TODO: change
                        }
                        break;

                    default:
                        await context.Write(description.Value.Slice(i, 1)/*code*/).ConfigureAwait(false); //TODO: change
                        break;
                }
            }

            await context.WriteLine().ConfigureAwait(false);
            await WriteIndent(context, level).ConfigureAwait(false);
            await context.Write("\"\"\"").ConfigureAwait(false);
            await context.WriteLine().ConfigureAwait(false);
        }

        async ValueTask WriteString()
        {
            int level = GetLevel(context);
            await WriteIndent(context, level).ConfigureAwait(false);
            await WriteEncodedString(context, description.Value).ConfigureAwait(false);
            await context.WriteLine().ConfigureAwait(false);
        }

        // http://spec.graphql.org/October2021/#StringValue
        if (ShouldBeMultilineBlockString())
            await WriteMultilineBlockString();
        else
            await WriteString();
    }

    /// <inheritdoc/>
    public override async ValueTask VisitName(GraphQLName name, TContext context)
    {
        await Visit(name.Comment, context).ConfigureAwait(false);
        await context.Write(name.Value).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, TContext context)
    {
        await Visit(fragmentDefinition.Comment, context).ConfigureAwait(false);
        await context.Write("fragment ").ConfigureAwait(false);
        await Visit(fragmentDefinition.Name, context).ConfigureAwait(false);
        await context.Write(" ").ConfigureAwait(false);
        await Visit(fragmentDefinition.TypeCondition, context).ConfigureAwait(false);
        await Visit(fragmentDefinition.Directives, context).ConfigureAwait(false);
        await Visit(fragmentDefinition.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, TContext context)
    {
        await Visit(fragmentSpread.Comment, context).ConfigureAwait(false);

        int level = GetLevel(context);
        await WriteIndent(context, level).ConfigureAwait(false);

        await context.Write("...").ConfigureAwait(false);
        await Visit(fragmentSpread.Name, context).ConfigureAwait(false);
        await Visit(fragmentSpread.Directives, context).ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInlineFragment(GraphQLInlineFragment inlineFragment, TContext context)
    {
        await Visit(inlineFragment.Comment, context).ConfigureAwait(false);

        int level = GetLevel(context);
        await WriteIndent(context, level).ConfigureAwait(false);

        await context.Write("... ").ConfigureAwait(false);
        await Visit(inlineFragment.TypeCondition, context).ConfigureAwait(false);
        await Visit(inlineFragment.Directives, context).ConfigureAwait(false);
        await Visit(inlineFragment.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitTypeCondition(GraphQLTypeCondition typeCondition, TContext context)
    {
        await Visit(typeCondition.Comment, context).ConfigureAwait(false);
        await context.Write("on ").ConfigureAwait(false);
        await Visit(typeCondition.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitImplementsInterfaces(GraphQLImplementsInterfaces implementsInterfaces, TContext context)
    {
        await context.Write(" implements ").ConfigureAwait(false);

        for (int i = 0; i < implementsInterfaces.Items.Count; ++i)
        {
            await Visit(implementsInterfaces.Items[i], context).ConfigureAwait(false);
            if (i < implementsInterfaces.Items.Count - 1)
                await context.Write(" & ").ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitSelectionSet(GraphQLSelectionSet selectionSet, TContext context)
    {
        await Visit(selectionSet.Comment, context).ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
        int level = GetLevel(context);
        await WriteIndent(context, level).ConfigureAwait(false);
        await context.Write("{").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);

        foreach (var selection in selectionSet.Selections)
            await Visit(selection, context).ConfigureAwait(false);

        await WriteIndent(context, level).ConfigureAwait(false);
        await context.Write("}").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitAlias(GraphQLAlias alias, TContext context)
    {
        await Visit(alias.Comment, context).ConfigureAwait(false);

        var level = GetLevel(context);
        await WriteIndent(context, level).ConfigureAwait(false);

        await Visit(alias.Name, context).ConfigureAwait(false);
        await context.Write(": ").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitField(GraphQLField field, TContext context)
    {
        await Visit(field.Comment, context).ConfigureAwait(false);
        await Visit(field.Alias, context).ConfigureAwait(false);
        if (field.Alias == null)
        {
            var level = GetLevel(context);
            await WriteIndent(context, level).ConfigureAwait(false);
        }
        await Visit(field.Name, context).ConfigureAwait(false);
        await Visit(field.Arguments, context).ConfigureAwait(false);
        await Visit(field.Directives, context).ConfigureAwait(false);

        if (field.SelectionSet == null)
            await context.WriteLine().ConfigureAwait(false);
        else
            await Visit(field.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, TContext context)
    {
        await Visit(operationDefinition.Comment, context).ConfigureAwait(false);
        if (operationDefinition.Name != null)
        {
            await context.Write(GetOperationType(operationDefinition.Operation)).ConfigureAwait(false);
            await context.Write(" ").ConfigureAwait(false);
            await Visit(operationDefinition.Name, context).ConfigureAwait(false);
        }
        await Visit(operationDefinition.Variables, context).ConfigureAwait(false);
        await Visit(operationDefinition.Directives, context).ConfigureAwait(false);
        await Visit(operationDefinition.SelectionSet, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, TContext context)
    {
        await Visit(directiveDefinition.Comment, context).ConfigureAwait(false);
        await Visit(directiveDefinition.Description, context).ConfigureAwait(false);
        await context.Write("directive @").ConfigureAwait(false);
        await Visit(directiveDefinition.Name, context).ConfigureAwait(false);
        await Visit(directiveDefinition.Arguments, context).ConfigureAwait(false);
        if (directiveDefinition.Repeatable)
            await context.Write(" repeatable").ConfigureAwait(false);
        await Visit(directiveDefinition.Locations, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDirectiveLocations(GraphQLDirectiveLocations directiveLocations, TContext context)
    {
        await context.Write(" on ").ConfigureAwait(false);
        for (int i = 0; i < directiveLocations.Items.Count; ++i)
        {
            await context.Write(GetDirectiveLocation(directiveLocations.Items[i])).ConfigureAwait(false);
            if (i < directiveLocations.Items.Count - 1)
                await context.Write(" | ").ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, TContext context)
    {
        await Visit(variableDefinition.Comment, context).ConfigureAwait(false);
        await Visit(variableDefinition.Variable, context).ConfigureAwait(false);
        await context.Write(": ").ConfigureAwait(false);
        await Visit(variableDefinition.Type, context).ConfigureAwait(false);
        if (variableDefinition.DefaultValue != null)
        {
            await context.Write(" = ").ConfigureAwait(false);
            await Visit(variableDefinition.DefaultValue, context).ConfigureAwait(false);
        }
        await Visit(variableDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitVariablesDefinition(GraphQLVariablesDefinition variablesDefinition, TContext context)
    {
        await context.Write("(").ConfigureAwait(false);

        for (int i = 0; i < variablesDefinition.Items.Count; ++i)
        {
            await Visit(variablesDefinition.Items[i], context).ConfigureAwait(false);
            if (i < variablesDefinition.Items.Count - 1)
                await context.Write(", ").ConfigureAwait(false);
        }

        await context.Write(")").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitVariable(GraphQLVariable variable, TContext context)
    {
        await Visit(variable.Comment, context).ConfigureAwait(false);
        await context.Write("$").ConfigureAwait(false);
        await Visit(variable.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitBooleanValue(GraphQLBooleanValue booleanValue, TContext context)
    {
        await Visit(booleanValue.Comment, context).ConfigureAwait(false);
        await context.Write(booleanValue.Value).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context)
    {
        await Visit(scalarTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(scalarTypeDefinition.Description, context).ConfigureAwait(false);
        await context.Write("scalar ").ConfigureAwait(false);
        await Visit(scalarTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(scalarTypeDefinition.Directives, context).ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitScalarTypeExtension(GraphQLScalarTypeExtension scalarTypeExtension, TContext context)
    {
        await Visit(scalarTypeExtension.Comment, context).ConfigureAwait(false);
        await context.Write("extend scalar ").ConfigureAwait(false);
        await Visit(scalarTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(scalarTypeExtension.Directives, context).ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context)
    {
        await Visit(enumTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(enumTypeDefinition.Description, context).ConfigureAwait(false);
        await context.Write("enum ").ConfigureAwait(false);
        await Visit(enumTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(enumTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(enumTypeDefinition.Values, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumTypeExtension(GraphQLEnumTypeExtension enumTypeExtension, TContext context)
    {
        await Visit(enumTypeExtension.Comment, context).ConfigureAwait(false);
        await context.Write("extend enum ").ConfigureAwait(false);
        await Visit(enumTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(enumTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(enumTypeExtension.Values, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, TContext context)
    {
        await Visit(enumValueDefinition.Comment, context).ConfigureAwait(false);
        await Visit(enumValueDefinition.Description, context).ConfigureAwait(false);

        int level = GetLevel(context);
        await WriteIndent(context, level).ConfigureAwait(false);

        await Visit(enumValueDefinition.Name, context).ConfigureAwait(false);
        await Visit(enumValueDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumValuesDefinition(GraphQLEnumValuesDefinition enumValuesDefinition, TContext context)
    {
        await context.WriteLine().ConfigureAwait(false);
        await context.Write("{").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);

        for (int i = 0; i < enumValuesDefinition.Items.Count; ++i)
        {
            await Visit(enumValuesDefinition.Items[i], context).ConfigureAwait(false);
            await context.WriteLine().ConfigureAwait(false);
        }

        await context.Write("}").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context)
    {
        await Visit(inputObjectTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(inputObjectTypeDefinition.Description, context).ConfigureAwait(false);
        await context.Write("input ").ConfigureAwait(false);
        await Visit(inputObjectTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(inputObjectTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(inputObjectTypeDefinition.Fields, context).ConfigureAwait(false);
        if (inputObjectTypeDefinition.Fields == null)
            await context.WriteLine().ConfigureAwait(false); // TODO: ???
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInputObjectTypeExtension(GraphQLInputObjectTypeExtension inputObjectTypeExtension, TContext context)
    {
        await Visit(inputObjectTypeExtension.Comment, context).ConfigureAwait(false);
        await context.Write("extend input ").ConfigureAwait(false);
        await Visit(inputObjectTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(inputObjectTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(inputObjectTypeExtension.Fields, context).ConfigureAwait(false);
        if (inputObjectTypeExtension.Fields == null)
            await context.WriteLine().ConfigureAwait(false); // TODO: ???
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, TContext context)
    {
        await Visit(inputValueDefinition.Comment, context).ConfigureAwait(false);
        await Visit(inputValueDefinition.Description, context).ConfigureAwait(false);

        int level = GetLevel(context);
        await WriteIndent(context, level).ConfigureAwait(false);

        await Visit(inputValueDefinition.Name, context).ConfigureAwait(false);
        await context.Write(": ").ConfigureAwait(false);
        await Visit(inputValueDefinition.Type, context).ConfigureAwait(false);
        if (inputValueDefinition.DefaultValue != null)
        {
            await context.Write(" = ").ConfigureAwait(false);
            await Visit(inputValueDefinition.DefaultValue, context).ConfigureAwait(false);
        }
        await Visit(inputValueDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInputFieldsDefinition(GraphQLInputFieldsDefinition inputFieldsDefinition, TContext context)
    {
        await context.WriteLine().ConfigureAwait(false);
        await context.Write("{").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);

        for (int i = 0; i < inputFieldsDefinition.Items.Count; ++i)
        {
            await Visit(inputFieldsDefinition.Items[i], context).ConfigureAwait(false);
            await context.WriteLine().ConfigureAwait(false);
        }

        await context.Write("}").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context)
    {
        await Visit(objectTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Description, context).ConfigureAwait(false);
        await context.Write("type ").ConfigureAwait(false);
        await Visit(objectTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Interfaces, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(objectTypeDefinition.Fields, context).ConfigureAwait(false);
        if (objectTypeDefinition.Fields == null)
            await context.WriteLine().ConfigureAwait(false); //TODO: ???
    }

    /// <inheritdoc/>
    public override async ValueTask VisitObjectTypeExtension(GraphQLObjectTypeExtension objectTypeExtension, TContext context)
    {
        await Visit(objectTypeExtension.Comment, context).ConfigureAwait(false);
        await context.Write("extend type ").ConfigureAwait(false);
        await Visit(objectTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(objectTypeExtension.Interfaces, context).ConfigureAwait(false);
        await Visit(objectTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(objectTypeExtension.Fields, context).ConfigureAwait(false);
        if (objectTypeExtension.Fields == null)
            await context.WriteLine().ConfigureAwait(false); // TODO: ???
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context)
    {
        await Visit(interfaceTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Description, context).ConfigureAwait(false);
        await context.Write("interface ").ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Interfaces, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(interfaceTypeDefinition.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitInterfaceTypeExtension(GraphQLInterfaceTypeExtension interfaceTypeExtension, TContext context)
    {
        await Visit(interfaceTypeExtension.Comment, context).ConfigureAwait(false);
        await context.Write("extend interface ").ConfigureAwait(false);
        await Visit(interfaceTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(interfaceTypeExtension.Interfaces, context).ConfigureAwait(false);
        await Visit(interfaceTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(interfaceTypeExtension.Fields, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, TContext context)
    {
        await Visit(fieldDefinition.Comment, context).ConfigureAwait(false);
        await Visit(fieldDefinition.Description, context).ConfigureAwait(false);

        int level = GetLevel(context);
        await WriteIndent(context, level).ConfigureAwait(false);

        await Visit(fieldDefinition.Name, context).ConfigureAwait(false);
        await Visit(fieldDefinition.Arguments, context).ConfigureAwait(false);
        await context.Write(": ").ConfigureAwait(false);
        await Visit(fieldDefinition.Type, context).ConfigureAwait(false);
        await Visit(fieldDefinition.Directives, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFieldsDefinition(GraphQLFieldsDefinition fieldsDefinition, TContext context)
    {
        await context.WriteLine().ConfigureAwait(false);
        await context.Write("{").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);

        for (int i = 0; i < fieldsDefinition.Items.Count; ++i)
        {
            await Visit(fieldsDefinition.Items[i], context).ConfigureAwait(false);
            await context.WriteLine().ConfigureAwait(false);
        }

        await context.Write("}").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, TContext context)
    {
        await Visit(schemaDefinition.Comment, context).ConfigureAwait(false);
        await Visit(schemaDefinition.Description, context).ConfigureAwait(false);
        await context.Write("schema").ConfigureAwait(false);
        await Visit(schemaDefinition.Directives, context).ConfigureAwait(false);

        await context.WriteLine().ConfigureAwait(false);
        await context.Write("{").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);

        if (schemaDefinition.OperationTypes.Count > 0)
        {
            for (int i = 0; i < schemaDefinition.OperationTypes.Count; ++i)
            {
                await Visit(schemaDefinition.OperationTypes[i], context).ConfigureAwait(false);
                await context.WriteLine().ConfigureAwait(false);
            }
        }

        await context.Write("}").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context)
    {
        await Visit(rootOperationTypeDefinition.Comment, context).ConfigureAwait(false);

        int level = GetLevel(context);
        await WriteIndent(context, level).ConfigureAwait(false);

        await context.Write(GetOperationType(rootOperationTypeDefinition.Operation)).ConfigureAwait(false);
        await context.Write(": ").ConfigureAwait(false);
        await Visit(rootOperationTypeDefinition.Type, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context)
    {
        await Visit(unionTypeDefinition.Comment, context).ConfigureAwait(false);
        await Visit(unionTypeDefinition.Description, context).ConfigureAwait(false);
        await context.Write("union ").ConfigureAwait(false);
        await Visit(unionTypeDefinition.Name, context).ConfigureAwait(false);
        await Visit(unionTypeDefinition.Directives, context).ConfigureAwait(false);
        await Visit(unionTypeDefinition.Types, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitUnionTypeExtension(GraphQLUnionTypeExtension unionTypeExtension, TContext context)
    {
        await Visit(unionTypeExtension.Comment, context).ConfigureAwait(false);
        await context.Write("extend union ").ConfigureAwait(false);
        await Visit(unionTypeExtension.Name, context).ConfigureAwait(false);
        await Visit(unionTypeExtension.Directives, context).ConfigureAwait(false);
        await Visit(unionTypeExtension.Types, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitUnionMemberTypes(GraphQLUnionMemberTypes unionMemberTypes, TContext context)
    {
        await context.Write(" = ").ConfigureAwait(false);

        for (int i = 0; i < unionMemberTypes.Items.Count; ++i)
        {
            await Visit(unionMemberTypes.Items[i], context).ConfigureAwait(false);
            if (i < unionMemberTypes.Items.Count - 1)
                await context.Write(" | ").ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDirective(GraphQLDirective directive, TContext context)
    {
        await Visit(directive.Comment, context).ConfigureAwait(false);
        await context.Write("@").ConfigureAwait(false);
        await Visit(directive.Name, context).ConfigureAwait(false);
        await Visit(directive.Arguments, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitDirectives(GraphQLDirectives directives, TContext context)
    {
        await context.Write(" ").ConfigureAwait(false);

        for (int i = 0; i < directives.Items.Count; ++i)
        {
            await Visit(directives.Items[i], context).ConfigureAwait(false);
            if (i < directives.Items.Count - 1)
                await context.Write(" ").ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public override async ValueTask VisitArgument(GraphQLArgument argument, TContext context)
    {
        await Visit(argument.Comment, context).ConfigureAwait(false);
        await Visit(argument.Name, context).ConfigureAwait(false);
        await context.Write(": ").ConfigureAwait(false);
        await Visit(argument.Value, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitArgumentsDefinition(GraphQLArgumentsDefinition argumentsDefinition, TContext context)
    {
        await context.Write("(").ConfigureAwait(false);
        await context.WriteLine().ConfigureAwait(false);

        for (int i = 0; i < argumentsDefinition.Items.Count; ++i)
        {
            await Visit(argumentsDefinition.Items[i], context).ConfigureAwait(false);
            if (i < argumentsDefinition.Items.Count - 1)
                await context.WriteLine().ConfigureAwait(false);
        }

        await context.WriteLine().ConfigureAwait(false);
        await context.Write(")").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitArguments(GraphQLArguments arguments, TContext context)
    {
        await context.Write("(").ConfigureAwait(false);
        for (int i = 0; i < arguments.Items.Count; ++i)
        {
            await Visit(arguments.Items[i], context).ConfigureAwait(false);
            if (i < arguments.Items.Count - 1)
                await context.Write(", ").ConfigureAwait(false);
        }
        await context.Write(")").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitNonNullType(GraphQLNonNullType nonNullType, TContext context)
    {
        await Visit(nonNullType.Comment, context).ConfigureAwait(false);
        await Visit(nonNullType.Type, context).ConfigureAwait(false);
        await context.Write("!").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitListType(GraphQLListType listType, TContext context)
    {
        await Visit(listType.Comment, context).ConfigureAwait(false);
        await context.Write("[").ConfigureAwait(false);
        await Visit(listType.Type, context).ConfigureAwait(false);
        await context.Write("]").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitListValue(GraphQLListValue listValue, TContext context)
    {
        await Visit(listValue.Comment, context).ConfigureAwait(false);
        await context.Write("[").ConfigureAwait(false);
        if (listValue.Values?.Count > 0)
        {
            for (int i = 0; i < listValue.Values.Count; ++i)
            {
                await Visit(listValue.Values[i], context).ConfigureAwait(false);
                if (i < listValue.Values.Count - 1)
                    await context.Write(", ").ConfigureAwait(false);
            }
        }
        await context.Write("]").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitNullValue(GraphQLNullValue nullValue, TContext context)
    {
        await Visit(nullValue.Comment, context).ConfigureAwait(false);
        await context.Write("null").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitStringValue(GraphQLStringValue stringValue, TContext context)
    {
        await Visit(stringValue.Comment, context).ConfigureAwait(false);
        await WriteEncodedString(context, stringValue.Value);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitIntValue(GraphQLIntValue intValue, TContext context)
    {
        await Visit(intValue.Comment, context).ConfigureAwait(false);
        await context.Write(intValue.Value).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitFloatValue(GraphQLFloatValue floatValue, TContext context)
    {
        await Visit(floatValue.Comment, context).ConfigureAwait(false);
        await context.Write(floatValue.Value).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitEnumValue(GraphQLEnumValue enumValue, TContext context)
    {
        await Visit(enumValue.Comment, context).ConfigureAwait(false);
        await Visit(enumValue.Name, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitObjectValue(GraphQLObjectValue objectValue, TContext context)
    {
        await Visit(objectValue.Comment, context).ConfigureAwait(false);
        await context.Write("{").ConfigureAwait(false);
        if (objectValue.Fields?.Count > 0)
        {
            for (int i = 0; i < objectValue.Fields.Count; ++i)
            {
                await Visit(objectValue.Fields[i], context).ConfigureAwait(false);
                if (i < objectValue.Fields.Count - 1)
                    await context.Write(", ").ConfigureAwait(false);
            }
        }
        await context.Write("}").ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async ValueTask VisitObjectField(GraphQLObjectField objectField, TContext context)
    {
        await Visit(objectField.Comment, context).ConfigureAwait(false);
        await Visit(objectField.Name, context).ConfigureAwait(false);
        await context.Write(": ").ConfigureAwait(false);
        await Visit(objectField.Value, context).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override ValueTask VisitNamedType(GraphQLNamedType namedType, TContext context)
    {
        return base.VisitNamedType(namedType, context);
    }

    /// <inheritdoc/>
    public override async ValueTask Visit(ASTNode? node, TContext context)
    {
        if (node == null)
            return;

        context.Parents.Push(node);

        await base.Visit(node, context).ConfigureAwait(false);

        context.Parents.Pop();
    }

    private static string GetOperationType(OperationType type) => type switch
    {
        OperationType.Query => "query",
        OperationType.Mutation => "mutation",
        OperationType.Subscription => "subscription",

        _ => throw new NotSupportedException(type.ToString()),
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

        _ => throw new NotSupportedException(location.ToString()),
    };

    private static async ValueTask WriteIndent(TContext context, int level)
    {
        for (int i = 0; i < level; ++i)
            await context.Write("  ").ConfigureAwait(false);
    }

    private static int GetLevel(TContext context)
    {
        int level = 0;

        if (context.Parents.Count > 0)
        {
            var currentNode = context.Parents.Pop();

            foreach (var node in context.Parents)
            {
                if (node is GraphQLSelectionSet ||
                    node is GraphQLTypeDefinition ||
                    node is GraphQLInputObjectTypeDefinition ||
                    node is GraphQLSchemaDefinition)
                    ++level;
            }

            if (currentNode is GraphQLDescription || currentNode is GraphQLDirectiveDefinition)
                --level;
            else if (currentNode is GraphQLComment && context.Parents.Peek() is GraphQLTypeDefinition)
                --level;

            context.Parents.Push(currentNode);
        }

        return level;
    }

    // http://spec.graphql.org/October2021/#StringCharacter
    private static async ValueTask WriteEncodedString(TContext context, ROM value)
    {
        await context.Write("\"").ConfigureAwait(false);

        for (int i = 0; i < value.Span.Length; ++i)
        {
            char code = value.Span[i];
            if (code < ' ')
            {
                if (code == '\b')
                    await context.Write("\\b").ConfigureAwait(false);
                else if (code == '\f')
                    await context.Write("\\f").ConfigureAwait(false);
                else if (code == '\n')
                    await context.Write("\\n").ConfigureAwait(false);
                else if (code == '\r')
                    await context.Write("\\r").ConfigureAwait(false);
                else if (code == '\t')
                    await context.Write("\\t").ConfigureAwait(false);
                else
                    await context.Write("\\u" + ((int)code).ToString("X4")).ConfigureAwait(false);
            }
            else if (code == '\\')
                await context.Write("\\\\").ConfigureAwait(false);
            else if (code == '"')
                await context.Write("\\\"").ConfigureAwait(false);
            else
                await context.Write(value.Slice(i, 1)/*code*/).ConfigureAwait(false); // TODO: no method for char
        }

        await context.Write("\"").ConfigureAwait(false);
    }
}
