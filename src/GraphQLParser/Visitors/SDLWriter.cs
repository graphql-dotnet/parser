using System.IO;
using System.Threading.Tasks;
using GraphQLParser.AST;

namespace GraphQLParser.Visitors
{
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
            if (document.Definitions?.Count > 0)
            {
                for (int i = 0; i < document.Definitions.Count; ++i)
                {
                    await Visit(document.Definitions[i], context);

                    if (i < document.Definitions.Count - 1)
                    {
                        await context.Writer.WriteLineAsync();
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
                    await WriteIndent(context, level);
                    await context.Writer.WriteAsync('#');
                    needStartNewLine = false;
                }

                char current = comment.Text.Span[i];
                switch (current)
                {
                    case '\r':
                        break;

                    case '\n':
                        await context.Writer.WriteLineAsync();
                        needStartNewLine = true;
                        break;

                    default:
                        await context.Writer.WriteAsync(current);
                        break;
                }
            }

            await context.Writer.WriteLineAsync();
        }

        /// <inheritdoc/>
        public override async ValueTask VisitDescription(GraphQLDescription description, TContext context)
        {
            int level = GetLevel(context);

            await WriteIndent(context, level);
            await context.Writer.WriteAsync("\"\"\"");
            await context.Writer.WriteLineAsync();

            bool needStartNewLine = true;
            int length = description.Value.Span.Length;
            for (int i = 0; i < length; ++i)
            {
                if (needStartNewLine)
                {
                    await WriteIndent(context, level);
                    needStartNewLine = false;
                }

                char current = description.Value.Span[i];
                switch (current)
                {
                    case '\r':
                        break;

                    case '\n':
                        await context.Writer.WriteLineAsync();
                        needStartNewLine = true;
                        break;

                    default:
                        await context.Writer.WriteAsync(current);
                        break;
                }
            }

            await context.Writer.WriteLineAsync();
            await WriteIndent(context, level);
            await context.Writer.WriteAsync("\"\"\"");
            await context.Writer.WriteLineAsync();
        }

        /// <inheritdoc/>
        public override async ValueTask VisitName(GraphQLName name, TContext context)
        {
            await Visit(name.Comment, context);
            await Write(context, name.Value);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, TContext context)
        {
            await Visit(fragmentDefinition.Comment, context);
            await context.Writer.WriteAsync("fragment ");
            await Visit(fragmentDefinition.Name, context);
            await context.Writer.WriteAsync(" ");
            await Visit(fragmentDefinition.TypeCondition, context);
            await VisitDirectives(fragmentDefinition, context);
            await Visit(fragmentDefinition.SelectionSet, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, TContext context)
        {
            await Visit(fragmentSpread.Comment, context);

            int level = GetLevel(context);
            await WriteIndent(context, level);

            await context.Writer.WriteAsync("...");
            await Visit(fragmentSpread.Name, context);
            await VisitDirectives(fragmentSpread, context);
            await context.Writer.WriteLineAsync();
        }

        /// <inheritdoc/>
        public override async ValueTask VisitInlineFragment(GraphQLInlineFragment inlineFragment, TContext context)
        {
            await Visit(inlineFragment.Comment, context);

            int level = GetLevel(context);
            await WriteIndent(context, level);

            await context.Writer.WriteAsync("... ");
            await Visit(inlineFragment.TypeCondition, context);
            await VisitDirectives(inlineFragment, context);
            await Visit(inlineFragment.SelectionSet, context);
        }

        public override async ValueTask VisitTypeCondition(GraphQLTypeCondition typeCondition, TContext context)
        {
            await Visit(typeCondition.Comment, context);
            await context.Writer.WriteAsync("on ");
            await Visit(typeCondition.Type, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitSelectionSet(GraphQLSelectionSet selectionSet, TContext context)
        {
            await Visit(selectionSet.Comment, context);
            await context.Writer.WriteLineAsync();
            int level = GetLevel(context);
            await WriteIndent(context, level);
            await context.Writer.WriteLineAsync('{');

            if (selectionSet.Selections?.Count > 0)
            {
                foreach (var selection in selectionSet.Selections)
                    await Visit(selection, context);
            }

            await WriteIndent(context, level);
            await context.Writer.WriteLineAsync('}');
        }

        /// <inheritdoc/>
        public override async ValueTask VisitField(GraphQLField field, TContext context)
        {
            await Visit(field.Comment, context);

            var level = GetLevel(context);
            await WriteIndent(context, level);

            if (field.Alias != null)
            {
                await Visit(field.Alias, context);
                await context.Writer.WriteAsync(": ");
            }
            await Visit(field.Name, context);
            if (field.Arguments != null)
            {
                await context.Writer.WriteAsync('(');
                await VisitArguments(field, context);
                await context.Writer.WriteAsync(')');
            }
            await VisitDirectives(field, context);

            if (field.SelectionSet == null)
                await context.Writer.WriteLineAsync();
            else
                await Visit(field.SelectionSet, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, TContext context)
        {
            await Visit(operationDefinition.Comment, context);

            if (operationDefinition.Name != null)
            {
                await context.Writer.WriteAsync(GetOperationType(operationDefinition.Operation));
                await context.Writer.WriteAsync(' ');
                await Visit(operationDefinition.Name, context);
            }

            if (operationDefinition.VariableDefinitions?.Count > 0)
            {
                await context.Writer.WriteAsync('(');
                if (operationDefinition.VariableDefinitions?.Count > 0)
                {
                    for (int i = 0; i < operationDefinition.VariableDefinitions.Count; ++i)
                    {
                        await Visit(operationDefinition.VariableDefinitions[i], context);
                        if (i < operationDefinition.VariableDefinitions.Count - 1)
                            await context.Writer.WriteAsync(", ");
                    }
                }
                await context.Writer.WriteAsync(')');
            }

            await VisitDirectives(operationDefinition, context);
            await Visit(operationDefinition.SelectionSet, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, TContext context)
        {
            await Visit(directiveDefinition.Comment, context);
            await Visit(directiveDefinition.Description, context);
            await context.Writer.WriteAsync("directive ");
            await Visit(directiveDefinition.Name, context);
            if (directiveDefinition.Arguments != null)
            {
                await context.Writer.WriteAsync('(');
                await Visit(directiveDefinition, context);
                await context.Writer.WriteAsync(')');
            }
        }

        /// <inheritdoc/>
        public override async ValueTask VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, TContext context)
        {
            await Visit(variableDefinition.Comment, context);
            await Visit(variableDefinition.Variable, context);
            await context.Writer.WriteAsync(": ");
            await Visit(variableDefinition.Type, context);
            if (variableDefinition.DefaultValue != null)
            {
                await context.Writer.WriteAsync(" = ");
                await Visit(variableDefinition.DefaultValue, context);
            }
            await VisitDirectives(variableDefinition, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitVariable(GraphQLVariable variable, TContext context)
        {
            await Visit(variable.Comment, context);
            await context.Writer.WriteAsync('$');
            await Visit(variable.Name, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitBooleanValue(GraphQLScalarValue booleanValue, TContext context)
        {
            await Visit(booleanValue.Comment, context);
            await Write(context, booleanValue.Value);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context)
        {
            await Visit(scalarTypeDefinition.Comment, context);
            await Visit(scalarTypeDefinition.Description, context);
            await context.Writer.WriteAsync("scalar ");
            await Visit(scalarTypeDefinition.Name, context);
            await VisitDirectives(scalarTypeDefinition, context);
            await context.Writer.WriteLineAsync();
        }

        /// <inheritdoc/>
        public override async ValueTask VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context)
        {
            await Visit(enumTypeDefinition.Comment, context);
            await Visit(enumTypeDefinition.Description, context);
            await context.Writer.WriteAsync("enum ");
            await Visit(enumTypeDefinition.Name, context);
            await VisitDirectives(enumTypeDefinition, context);
            if (enumTypeDefinition.Values?.Count > 0)
            {
                await context.Writer.WriteLineAsync();
                await context.Writer.WriteLineAsync('{');
                for (int i = 0; i < enumTypeDefinition.Values.Count; ++i)
                {
                    await Visit(enumTypeDefinition.Values[i], context);
                    await context.Writer.WriteLineAsync();
                }
                await context.Writer.WriteLineAsync('}');
            }
        }

        /// <inheritdoc/>
        public override async ValueTask VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, TContext context)
        {
            await Visit(enumValueDefinition.Comment, context);
            await Visit(enumValueDefinition.Description, context);

            int level = GetLevel(context);
            await WriteIndent(context, level);

            await Visit(enumValueDefinition.Name, context);
            await VisitDirectives(enumValueDefinition, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context)
        {
            await Visit(inputObjectTypeDefinition.Comment, context);
            await Visit(inputObjectTypeDefinition.Description, context);
            await context.Writer.WriteAsync("input ");
            await Visit(inputObjectTypeDefinition.Name, context);
            await VisitDirectives(inputObjectTypeDefinition, context);
            if (inputObjectTypeDefinition.Fields?.Count > 0)
            {
                await context.Writer.WriteLineAsync();
                await context.Writer.WriteLineAsync('{');

                for (int i = 0; i < inputObjectTypeDefinition.Fields.Count; ++i)
                {
                    await Visit(inputObjectTypeDefinition.Fields[i], context);
                    await context.Writer.WriteLineAsync();
                }

                await context.Writer.WriteLineAsync('}');
            }
            else
            {
                await context.Writer.WriteLineAsync();
            }
        }

        /// <inheritdoc/>
        public override async ValueTask VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, TContext context)
        {
            await Visit(inputValueDefinition.Comment, context);
            await Visit(inputValueDefinition.Description, context);

            int level = GetLevel(context);
            await WriteIndent(context, level);

            await Visit(inputValueDefinition.Name, context);
            await context.Writer.WriteAsync(": ");
            await Visit(inputValueDefinition.Type, context);
            if (inputValueDefinition.DefaultValue != null)
            {
                await context.Writer.WriteAsync(" = ");
                await Visit(inputValueDefinition.DefaultValue, context);
            }
            await VisitDirectives(inputValueDefinition, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context)
        {
            await Visit(objectTypeDefinition.Comment, context);
            await Visit(objectTypeDefinition.Description, context);
            await context.Writer.WriteAsync("type ");
            await Visit(objectTypeDefinition.Name, context);
            await VisitInterfaces(objectTypeDefinition, context);
            await VisitDirectives(objectTypeDefinition, context);

            if (objectTypeDefinition.Fields?.Count > 0)
            {
                await context.Writer.WriteLineAsync();
                await context.Writer.WriteLineAsync('{');

                for (int i = 0; i < objectTypeDefinition.Fields.Count; ++i)
                {
                    await Visit(objectTypeDefinition.Fields[i], context);
                    await context.Writer.WriteLineAsync();
                }

                await context.Writer.WriteLineAsync('}');
            }
            else
            {
                await context.Writer.WriteLineAsync();
            }
        }

        /// <inheritdoc/>
        public override async ValueTask VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context)
        {
            await Visit(interfaceTypeDefinition.Comment, context);
            await Visit(interfaceTypeDefinition.Description, context);
            await context.Writer.WriteAsync("interface ");
            await Visit(interfaceTypeDefinition.Name, context);
            await VisitInterfaces(interfaceTypeDefinition, context);
            await VisitDirectives(interfaceTypeDefinition, context);
            await context.Writer.WriteLineAsync();
            await context.Writer.WriteLineAsync('{');

            if (interfaceTypeDefinition.Fields?.Count > 0)
            {
                for (int i = 0; i < interfaceTypeDefinition.Fields.Count; ++i)
                {
                    await Visit(interfaceTypeDefinition.Fields[i], context);
                    await context.Writer.WriteLineAsync();
                }
            }

            await context.Writer.WriteLineAsync('}');
        }

        /// <inheritdoc/>
        public override async ValueTask VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, TContext context)
        {
            await Visit(fieldDefinition.Comment, context);
            await Visit(fieldDefinition.Description, context);

            int level = GetLevel(context);
            await WriteIndent(context, level);

            await Visit(fieldDefinition.Name, context);
            if (fieldDefinition.Arguments?.Count > 0)
            {
                await context.Writer.WriteAsync('(');
                for (int i = 0; i < fieldDefinition.Arguments.Count; ++i)
                {
                    await Visit(fieldDefinition.Arguments[i], context);
                    if (i < fieldDefinition.Arguments.Count - 1)
                        await context.Writer.WriteAsync(", ");
                }
                await context.Writer.WriteAsync(')');
            }
            await context.Writer.WriteAsync(": ");
            await Visit(fieldDefinition.Type, context);
            await VisitDirectives(fieldDefinition, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, TContext context)
        {
            await Visit(schemaDefinition.Comment, context);
            await Visit(schemaDefinition.Description, context);
            await context.Writer.WriteAsync("schema");
            await VisitDirectives(schemaDefinition, context);

            await context.Writer.WriteLineAsync();
            await context.Writer.WriteLineAsync('{');

            if (schemaDefinition.OperationTypes?.Count > 0)
            {
                for (int i = 0; i < schemaDefinition.OperationTypes.Count; ++i)
                {
                    await Visit(schemaDefinition.OperationTypes[i], context);
                    await context.Writer.WriteLineAsync();
                }
            }

            await context.Writer.WriteLineAsync('}');
        }

        /// <inheritdoc/>
        public override async ValueTask VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context)
        {
            await Visit(rootOperationTypeDefinition.Comment, context);

            int level = GetLevel(context);
            await WriteIndent(context, level);

            await context.Writer.WriteAsync(GetOperationType(rootOperationTypeDefinition.Operation));
            await context.Writer.WriteAsync(": ");
            await Visit(rootOperationTypeDefinition.Type, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context)
        {
            await Visit(unionTypeDefinition.Comment, context);
            await Visit(unionTypeDefinition.Description, context);
            await context.Writer.WriteAsync("union ");
            await Visit(unionTypeDefinition.Name, context);
            await VisitDirectives(unionTypeDefinition, context);

            if (unionTypeDefinition.Types?.Count > 0)
            {
                await context.Writer.WriteAsync(" = ");

                for (int i = 0; i < unionTypeDefinition.Types.Count; ++i)
                {
                    await Visit(unionTypeDefinition.Types[i], context);
                    if (i < unionTypeDefinition.Types.Count - 1)
                        await context.Writer.WriteAsync(" | ");
                }
            }
        }

        /// <inheritdoc/>
        public override async ValueTask VisitDirective(GraphQLDirective directive, TContext context)
        {
            await Visit(directive.Comment, context);
            await context.Writer.WriteAsync('@');
            await Visit(directive.Name, context);
            if (directive.Arguments != null)
            {
                await context.Writer.WriteAsync('(');
                await VisitArguments(directive, context);
                await context.Writer.WriteAsync(')');
            }
        }

        /// <inheritdoc/>
        public override async ValueTask VisitArgument(GraphQLArgument argument, TContext context)
        {
            await Visit(argument.Comment, context);
            await Visit(argument.Name, context);
            await context.Writer.WriteAsync(": ");
            await Visit(argument.Value, context);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitNonNullType(GraphQLNonNullType nonNullType, TContext context)
        {
            await Visit(nonNullType.Comment, context);
            await Visit(nonNullType.Type, context);
            await context.Writer.WriteAsync('!');
        }

        /// <inheritdoc/>
        public override async ValueTask VisitListType(GraphQLListType listType, TContext context)
        {
            await Visit(listType.Comment, context);
            await context.Writer.WriteAsync('[');
            await Visit(listType.Type, context);
            await context.Writer.WriteAsync(']');
        }

        /// <inheritdoc/>
        public override async ValueTask VisitListValue(GraphQLListValue listValue, TContext context)
        {
            await Visit(listValue.Comment, context);
            if (listValue.Values?.Count > 0)
            {
                await context.Writer.WriteAsync('[');
                for (int i = 0; i < listValue.Values.Count; ++i)
                {
                    await Visit(listValue.Values[i], context);
                    if (i < listValue.Values.Count - 1)
                        await context.Writer.WriteAsync(", ");
                }
                await context.Writer.WriteAsync(']');
            }
        }

        /// <inheritdoc/>
        public override async ValueTask VisitNullValue(GraphQLScalarValue nullValue, TContext context)
        {
            await Visit(nullValue.Comment, context);
            await context.Writer.WriteAsync("null");
        }

        /// <inheritdoc/>
        public override async ValueTask VisitStringValue(GraphQLScalarValue stringValue, TContext context)
        {
            await Visit(stringValue.Comment, context);
            await context.Writer.WriteAsync('\"');
            await Write(context, stringValue.Value);
            await context.Writer.WriteAsync('\"');
        }

        /// <inheritdoc/>
        public override async ValueTask VisitIntValue(GraphQLScalarValue intValue, TContext context)
        {
            await Visit(intValue.Comment, context);
            await Write(context, intValue.Value);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitFloatValue(GraphQLScalarValue floatValue, TContext context)
        {
            await Visit(floatValue.Comment, context);
            await Write(context, floatValue.Value);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitEnumValue(GraphQLScalarValue enumValue, TContext context)
        {
            await Visit(enumValue.Comment, context);
            await Write(context, enumValue.Value);
        }

        /// <inheritdoc/>
        public override async ValueTask VisitObjectValue(GraphQLObjectValue objectValue, TContext context)
        {
            await Visit(objectValue.Comment, context);

            if (objectValue.Fields?.Count > 0)
            {
                await context.Writer.WriteAsync("{ ");
                for (int i = 0; i < objectValue.Fields.Count; ++i)
                {
                    await Visit(objectValue.Fields[i], context);
                    if (i < objectValue.Fields.Count - 1)
                        await context.Writer.WriteAsync(", ");
                }
                await context.Writer.WriteAsync(" }");
            }
            else
            {
                await context.Writer.WriteAsync("{ }");
            }
        }

        /// <inheritdoc/>
        public override async ValueTask VisitObjectField(GraphQLObjectField objectField, TContext context)
        {
            await Visit(objectField.Comment, context);
            await Visit(objectField.Name, context);
            await context.Writer.WriteAsync(": ");
            await Visit(objectField.Value, context);
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

            context.Parent.Push(node);

            await base.Visit(node, context);

            context.Parent.Pop();
        }

        private async ValueTask VisitArguments(IHasArgumentsNode node, TContext context)
        {
            if (node.Arguments?.Count > 0)
            {
                for (int i = 0; i < node.Arguments.Count; ++i)
                {
                    await Visit(node.Arguments[i], context);
                    if (i < node.Arguments.Count - 1)
                        await context.Writer.WriteAsync(", ");
                }
            }
        }

        private async ValueTask VisitInterfaces(IHasInterfacesNode node, TContext context)
        {
            if (node.Interfaces?.Count > 0)
            {
                await context.Writer.WriteAsync(" implements ");

                for (int i = 0; i < node.Interfaces.Count; ++i)
                {
                    await Visit(node.Interfaces[i], context);
                    if (i < node.Interfaces.Count - 1)
                        await context.Writer.WriteAsync(" & ");
                }
            }
        }

        private async ValueTask VisitDirectives(IHasDirectivesNode node, TContext context)
        {
            if (node.Directives?.Count > 0)
            {
                await context.Writer.WriteAsync(' ');

                for (int i = 0; i < node.Directives.Count; ++i)
                {
                    await Visit(node.Directives[i], context);
                    if (i < node.Directives.Count - 1)
                        await context.Writer.WriteAsync(' ');
                }
            }
        }

        private string GetOperationType(OperationType type) => type switch
        {
            OperationType.Mutation => "mutation",
            OperationType.Subscription => "subscription",
            _ => "query",
        };

        private async ValueTask WriteIndent(TContext context, int level)
        {
            for (int i = 0; i < level; ++i)
                await context.Writer.WriteAsync("  ");
        }

        private int GetLevel(TContext context)
        {
            int level = 0;

            if (context.Parent.Count > 0)
            {
                var currentNode = context.Parent.Pop();

                foreach (var node in context.Parent)
                {
                    if (node is GraphQLSelectionSet ||
                        node is GraphQLTypeDefinition ||
                        node is GraphQLInputObjectTypeDefinition ||
                        node is GraphQLSchemaDefinition)
                        ++level;
                }

                if (currentNode is GraphQLDescription)
                    --level;
                else if (currentNode is GraphQLComment && context.Parent.Peek() is GraphQLTypeDefinition)
                    --level;

                context.Parent.Push(currentNode);
            }

            return level;
        }

        private ValueTask Write(TContext context, ROM value)
        {
            var task =
#if NETSTANDARD2_0
            context.Writer.WriteAsync(value.ToString());
#elif NETSTANDARD2_1_OR_GREATER
            context.Writer.WriteAsync(value);
#endif
            return new ValueTask(task);
        }
    }
}
