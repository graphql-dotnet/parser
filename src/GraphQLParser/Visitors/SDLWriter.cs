using System.Collections.Generic;
using System.IO;
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
        public override void VisitDocument(GraphQLDocument document, TContext context)
        {
            if (document.Definitions?.Count > 0)
            {
                for (int i = 0; i < document.Definitions.Count; ++i)
                {
                    Visit(document.Definitions[i], context);

                    if (i < document.Definitions.Count - 1)
                    {
                        context.Writer.WriteLine();
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void VisitComment(GraphQLComment comment, TContext context)
        {
            int level = GetLevel(context);

            bool needStartNewLine = true;
            var span = comment.Text.Span;
            for (int i = 0; i < span.Length; ++i)
            {
                if (needStartNewLine)
                {
                    WriteIndent(context, level);
                    context.Writer.Write('#');
                    needStartNewLine = false;
                }

                switch (span[i])
                {
                    case '\r':
                        break;

                    case '\n':
                        context.Writer.WriteLine();
                        needStartNewLine = true;
                        break;

                    default:
                        context.Writer.Write(span[i]);
                        break;
                }
            }

            context.Writer.WriteLine();
        }

        /// <inheritdoc/>
        public override void VisitDescription(GraphQLDescription description, TContext context)
        {
            int level = GetLevel(context);

            WriteIndent(context, level);
            context.Writer.Write("\"\"\"");
            context.Writer.WriteLine();

            bool needStartNewLine = true;
            var span = description.Value.Span;
            for (int i = 0; i < span.Length; ++i)
            {
                if (needStartNewLine)
                {
                    WriteIndent(context, level);
                    needStartNewLine = false;
                }

                switch (span[i])
                {
                    case '\r':
                        break;

                    case '\n':
                        context.Writer.WriteLine();
                        needStartNewLine = true;
                        break;

                    default:
                        context.Writer.Write(span[i]);
                        break;
                }
            }

            context.Writer.WriteLine();
            WriteIndent(context, level);
            context.Writer.Write("\"\"\"");
            context.Writer.WriteLine();
        }

        /// <inheritdoc/>
        public override void VisitName(GraphQLName name, TContext context)
        {
            Visit(name.Comment, context);
            Write(context, name.Value);
        }

        /// <inheritdoc/>
        public override void VisitFragmentDefinition(GraphQLFragmentDefinition fragmentDefinition, TContext context)
        {
            Visit(fragmentDefinition.Comment, context);
            context.Writer.Write("fragment ");
            Visit(fragmentDefinition.Name, context);
            context.Writer.Write(" on ");
            Visit(fragmentDefinition.TypeCondition, context);
            VisitDirectives(fragmentDefinition, context);
            Visit(fragmentDefinition.SelectionSet, context);
        }

        /// <inheritdoc/>
        public override void VisitFragmentSpread(GraphQLFragmentSpread fragmentSpread, TContext context)
        {
            Visit(fragmentSpread.Comment, context);

            int level = GetLevel(context);
            WriteIndent(context, level);

            context.Writer.Write("...");
            Visit(fragmentSpread.Name, context);
            VisitDirectives(fragmentSpread, context);
            context.Writer.WriteLine();
        }

        /// <inheritdoc/>
        public override void VisitInlineFragment(GraphQLInlineFragment inlineFragment, TContext context)
        {
            Visit(inlineFragment.Comment, context);

            int level = GetLevel(context);
            WriteIndent(context, level);

            context.Writer.Write("... on ");
            Visit(inlineFragment.TypeCondition, context);
            VisitDirectives(inlineFragment, context);
            Visit(inlineFragment.SelectionSet, context);
        }

        /// <inheritdoc/>
        public override void VisitSelectionSet(GraphQLSelectionSet selectionSet, TContext context)
        {
            Visit(selectionSet.Comment, context);
            context.Writer.WriteLine();
            int level = GetLevel(context);
            WriteIndent(context, level);
            context.Writer.WriteLine('{');

            if (selectionSet.Selections?.Count > 0)
            {
                foreach (var selection in selectionSet.Selections)
                    Visit(selection, context);
            }

            WriteIndent(context, level);
            context.Writer.WriteLine('}');
        }

        /// <inheritdoc/>
        public override void VisitField(GraphQLField field, TContext context)
        {
            Visit(field.Comment, context);

            var level = GetLevel(context);
            WriteIndent(context, level);

            if (field.Alias != null)
            {
                Visit(field.Alias, context);
                context.Writer.Write(": ");
            }
            Visit(field.Name, context);
            if (field.Arguments != null)
            {
                context.Writer.Write('(');
                VisitArguments(field, context);
                context.Writer.Write(')');
            }
            VisitDirectives(field, context);

            if (field.SelectionSet == null)
                context.Writer.WriteLine();
            else
                Visit(field.SelectionSet, context);
        }

        /// <inheritdoc/>
        public override void VisitOperationDefinition(GraphQLOperationDefinition operationDefinition, TContext context)
        {
            Visit(operationDefinition.Comment, context);

            if (operationDefinition.Name != null)
            {
                context.Writer.Write(GetOperationType(operationDefinition.Operation));
                context.Writer.Write(' ');
                Visit(operationDefinition.Name, context);
            }

            if (operationDefinition.VariableDefinitions?.Count > 0)
            {
                context.Writer.Write('(');
                Visit(operationDefinition.VariableDefinitions, context, ", ", "");
                context.Writer.Write(')');
            }

            VisitDirectives(operationDefinition, context);
            Visit(operationDefinition.SelectionSet, context);
        }

        /// <inheritdoc/>
        public override void VisitDirectiveDefinition(GraphQLDirectiveDefinition directiveDefinition, TContext context)
        {
            Visit(directiveDefinition.Comment, context);
            Visit(directiveDefinition.Description, context);
            context.Writer.Write("directive ");
            Visit(directiveDefinition.Name, context);
            if (directiveDefinition.Arguments != null)
            {
                context.Writer.Write('(');
                Visit(directiveDefinition, context);
                context.Writer.Write(')');
            }
        }

        /// <inheritdoc/>
        public override void VisitVariableDefinition(GraphQLVariableDefinition variableDefinition, TContext context)
        {
            Visit(variableDefinition.Comment, context);
            Visit(variableDefinition.Variable, context);
            context.Writer.Write(": ");
            Visit(variableDefinition.Type, context);
            if (variableDefinition.DefaultValue != null)
            {
                context.Writer.Write(" = ");
                Visit(variableDefinition.DefaultValue, context);
            }
            VisitDirectives(variableDefinition, context);
        }

        /// <inheritdoc/>
        public override void VisitVariable(GraphQLVariable variable, TContext context)
        {
            Visit(variable.Comment, context);
            context.Writer.Write('$');
            Visit(variable.Name, context);
        }

        /// <inheritdoc/>
        public override void VisitBooleanValue(GraphQLScalarValue booleanValue, TContext context)
        {
            Visit(booleanValue.Comment, context);
            Write(context, booleanValue.Value);
        }

        /// <inheritdoc/>
        public override void VisitScalarTypeDefinition(GraphQLScalarTypeDefinition scalarTypeDefinition, TContext context)
        {
            Visit(scalarTypeDefinition.Comment, context);
            Visit(scalarTypeDefinition.Description, context);
            context.Writer.Write("scalar ");
            Visit(scalarTypeDefinition.Name, context);
            VisitDirectives(scalarTypeDefinition, context);
            context.Writer.WriteLine();
        }

        /// <inheritdoc/>
        public override void VisitEnumTypeDefinition(GraphQLEnumTypeDefinition enumTypeDefinition, TContext context)
        {
            Visit(enumTypeDefinition.Comment, context);
            Visit(enumTypeDefinition.Description, context);
            context.Writer.Write("enum ");
            Visit(enumTypeDefinition.Name, context);
            VisitDirectives(enumTypeDefinition, context);
            if (enumTypeDefinition.Values?.Count > 0)
            {
                context.Writer.WriteLine();
                context.Writer.WriteLine('{');
                for (int i = 0; i < enumTypeDefinition.Values.Count; ++i)
                {
                    Visit(enumTypeDefinition.Values[i], context);
                    context.Writer.WriteLine();
                }
                context.Writer.WriteLine('}');
            }
        }

        /// <inheritdoc/>
        public override void VisitEnumValueDefinition(GraphQLEnumValueDefinition enumValueDefinition, TContext context)
        {
            Visit(enumValueDefinition.Comment, context);
            Visit(enumValueDefinition.Description, context);

            int level = GetLevel(context);
            WriteIndent(context, level);

            Visit(enumValueDefinition.Name, context);
            VisitDirectives(enumValueDefinition, context);
        }

        /// <inheritdoc/>
        public override void VisitInputObjectTypeDefinition(GraphQLInputObjectTypeDefinition inputObjectTypeDefinition, TContext context)
        {
            Visit(inputObjectTypeDefinition.Comment, context);
            Visit(inputObjectTypeDefinition.Description, context);
            context.Writer.Write("input ");
            Visit(inputObjectTypeDefinition.Name, context);
            VisitDirectives(inputObjectTypeDefinition, context);
            if (inputObjectTypeDefinition.Fields?.Count > 0)
            {
                context.Writer.WriteLine();
                context.Writer.WriteLine('{');

                for (int i = 0; i < inputObjectTypeDefinition.Fields.Count; ++i)
                {
                    Visit(inputObjectTypeDefinition.Fields[i], context);
                    context.Writer.WriteLine();
                }

                context.Writer.WriteLine('}');
            }
            else
            {
                context.Writer.WriteLine();
            }
        }

        /// <inheritdoc/>
        public override void VisitInputValueDefinition(GraphQLInputValueDefinition inputValueDefinition, TContext context)
        {
            Visit(inputValueDefinition.Comment, context);
            Visit(inputValueDefinition.Description, context);

            int level = GetLevel(context);
            WriteIndent(context, level);

            Visit(inputValueDefinition.Name, context);
            context.Writer.Write(": ");
            Visit(inputValueDefinition.Type, context);
            if (inputValueDefinition.DefaultValue != null)
            {
                context.Writer.Write(" = ");
                Visit(inputValueDefinition.DefaultValue, context);
            }
            VisitDirectives(inputValueDefinition, context);
        }

        /// <inheritdoc/>
        public override void VisitObjectTypeDefinition(GraphQLObjectTypeDefinition objectTypeDefinition, TContext context)
        {
            Visit(objectTypeDefinition.Comment, context);
            Visit(objectTypeDefinition.Description, context);
            context.Writer.Write("type ");
            Visit(objectTypeDefinition.Name, context);
            VisitInterfaces(objectTypeDefinition, context);
            VisitDirectives(objectTypeDefinition, context);

            if (objectTypeDefinition.Fields?.Count > 0)
            {
                context.Writer.WriteLine();
                context.Writer.WriteLine('{');

                for (int i = 0; i < objectTypeDefinition.Fields.Count; ++i)
                {
                    Visit(objectTypeDefinition.Fields[i], context);
                    context.Writer.WriteLine();
                }

                context.Writer.WriteLine('}');
            }
            else
            {
                context.Writer.WriteLine();
            }
        }

        /// <inheritdoc/>
        public override void VisitInterfaceTypeDefinition(GraphQLInterfaceTypeDefinition interfaceTypeDefinition, TContext context)
        {
            Visit(interfaceTypeDefinition.Comment, context);
            Visit(interfaceTypeDefinition.Description, context);
            context.Writer.Write("interface ");
            Visit(interfaceTypeDefinition.Name, context);
            VisitInterfaces(interfaceTypeDefinition, context);
            VisitDirectives(interfaceTypeDefinition, context);
            context.Writer.WriteLine();
            context.Writer.WriteLine('{');

            if (interfaceTypeDefinition.Fields?.Count > 0)
            {
                for (int i = 0; i < interfaceTypeDefinition.Fields.Count; ++i)
                {
                    Visit(interfaceTypeDefinition.Fields[i], context);
                    context.Writer.WriteLine();
                }
            }

            context.Writer.WriteLine('}');
        }

        /// <inheritdoc/>
        public override void VisitFieldDefinition(GraphQLFieldDefinition fieldDefinition, TContext context)
        {
            Visit(fieldDefinition.Comment, context);
            Visit(fieldDefinition.Description, context);

            int level = GetLevel(context);
            WriteIndent(context, level);

            Visit(fieldDefinition.Name, context);
            if (fieldDefinition.Arguments?.Count > 0)
            {
                context.Writer.Write('(');
                for (int i = 0; i < fieldDefinition.Arguments.Count; ++i)
                {
                    Visit(fieldDefinition.Arguments[i], context);
                    if (i < fieldDefinition.Arguments.Count - 1)
                        context.Writer.Write(", ");
                }
                context.Writer.Write(')');
            }
            context.Writer.Write(": ");
            Visit(fieldDefinition.Type, context);
            VisitDirectives(fieldDefinition, context);
        }

        /// <inheritdoc/>
        public override void VisitSchemaDefinition(GraphQLSchemaDefinition schemaDefinition, TContext context)
        {
            Visit(schemaDefinition.Comment, context);
            Visit(schemaDefinition.Description, context);
            context.Writer.Write("schema");
            VisitDirectives(schemaDefinition, context);

            context.Writer.WriteLine();
            context.Writer.WriteLine('{');

            if (schemaDefinition.OperationTypes?.Count > 0)
            {
                for (int i = 0; i < schemaDefinition.OperationTypes.Count; ++i)
                {
                    Visit(schemaDefinition.OperationTypes[i], context);
                    context.Writer.WriteLine();
                }
            }

            context.Writer.WriteLine('}');
        }

        /// <inheritdoc/>
        public override void VisitRootOperationTypeDefinition(GraphQLRootOperationTypeDefinition rootOperationTypeDefinition, TContext context)
        {
            Visit(rootOperationTypeDefinition.Comment, context);

            int level = GetLevel(context);
            WriteIndent(context, level);

            context.Writer.Write(GetOperationType(rootOperationTypeDefinition.Operation));
            context.Writer.Write(": ");
            Visit(rootOperationTypeDefinition.Type, context);
        }

        /// <inheritdoc/>
        public override void VisitUnionTypeDefinition(GraphQLUnionTypeDefinition unionTypeDefinition, TContext context)
        {
            Visit(unionTypeDefinition.Comment, context);
            Visit(unionTypeDefinition.Description, context);
            context.Writer.Write("union ");
            Visit(unionTypeDefinition.Name, context);
            VisitDirectives(unionTypeDefinition, context);

            if (unionTypeDefinition.Types?.Count > 0)
            {
                context.Writer.Write(" = ");

                for (int i = 0; i < unionTypeDefinition.Types.Count; ++i)
                {
                    Visit(unionTypeDefinition.Types[i], context);
                    if (i < unionTypeDefinition.Types.Count - 1)
                        context.Writer.Write(" | ");
                }
            }
        }

        /// <inheritdoc/>
        public override void VisitDirective(GraphQLDirective directive, TContext context)
        {
            Visit(directive.Comment, context);
            context.Writer.Write('@');
            Visit(directive.Name, context);
            if (directive.Arguments != null)
            {
                context.Writer.Write('(');
                VisitArguments(directive, context);
                context.Writer.Write(')');
            }
        }

        /// <inheritdoc/>
        public override void VisitArgument(GraphQLArgument argument, TContext context)
        {
            Visit(argument.Comment, context);
            Visit(argument.Name, context);
            context.Writer.Write(": ");
            Visit(argument.Value, context);
        }

        /// <inheritdoc/>
        public override void VisitNonNullType(GraphQLNonNullType nonNullType, TContext context)
        {
            Visit(nonNullType.Comment, context);
            Visit(nonNullType.Type, context);
            context.Writer.Write('!');
        }

        /// <inheritdoc/>
        public override void VisitListType(GraphQLListType listType, TContext context)
        {
            Visit(listType.Comment, context);
            context.Writer.Write('[');
            Visit(listType.Type, context);
            context.Writer.Write(']');
        }

        /// <inheritdoc/>
        public override void VisitListValue(GraphQLListValue listValue, TContext context)
        {
            Visit(listValue.Comment, context);
            if (listValue.Values?.Count > 0)
            {
                context.Writer.Write('[');
                for (int i=0; i<listValue.Values.Count; ++i)
                {
                    Visit(listValue.Values[i], context);
                    if (i < listValue.Values.Count - 1)
                        context.Writer.Write(", ");
                }
                context.Writer.Write(']');
            }
        }

        /// <inheritdoc/>
        public override void VisitNullValue(GraphQLScalarValue nullValue, TContext context)
        {
            Visit(nullValue.Comment, context);
            context.Writer.Write("null");
        }

        /// <inheritdoc/>
        public override void VisitStringValue(GraphQLScalarValue stringValue, TContext context)
        {
            Visit(stringValue.Comment, context);
            context.Writer.Write('\"');
            Write(context, stringValue.Value);
            context.Writer.Write('\"');
        }

        /// <inheritdoc/>
        public override void VisitIntValue(GraphQLScalarValue intValue, TContext context)
        {
            Visit(intValue.Comment, context);
            Write(context, intValue.Value);
        }

        /// <inheritdoc/>
        public override void VisitFloatValue(GraphQLScalarValue floatValue, TContext context)
        {
            Visit(floatValue.Comment, context);
            Write(context, floatValue.Value);
        }

        /// <inheritdoc/>
        public override void VisitEnumValue(GraphQLScalarValue enumValue, TContext context)
        {
            Visit(enumValue.Comment, context);
            Write(context, enumValue.Value);
        }

        /// <inheritdoc/>
        public override void VisitObjectValue(GraphQLObjectValue objectValue, TContext context)
        {
            Visit(objectValue.Comment, context);

            if (objectValue.Fields?.Count > 0)
            {
                context.Writer.Write("{ ");
                for (int i = 0; i < objectValue.Fields.Count; ++i)
                {
                    Visit(objectValue.Fields[i], context);
                    if (i < objectValue.Fields.Count - 1)
                        context.Writer.Write(", ");
                }
                context.Writer.Write(" }");
            }
            else
            {
                context.Writer.Write("{ }");
            }
        }

        /// <inheritdoc/>
        public override void VisitObjectField(GraphQLObjectField objectField, TContext context)
        {
            Visit(objectField.Comment, context);
            Visit(objectField.Name, context);
            context.Writer.Write(": ");
            Visit(objectField.Value, context);
        }

        /// <inheritdoc/>
        public override void VisitNamedType(GraphQLNamedType namedType, TContext context)
        {
            base.VisitNamedType(namedType, context);
        }

        /// <inheritdoc/>
        public override void Visit(ASTNode? node, TContext context)
        {
            if (node == null)
                return;

            context.Parent.Push(node);

            base.Visit(node, context);

            context.Parent.Pop();
        }

        private void VisitArguments(IHasArgumentsNode node, TContext context)
        {
            if (node.Arguments?.Count > 0)
            {
                for (int i = 0; i < node.Arguments.Count; ++i)
                {
                    Visit(node.Arguments[i], context);
                    if (i < node.Arguments.Count - 1)
                        context.Writer.Write(", ");
                }
            }
        }

        private void VisitInterfaces(IHasInterfacesNode node, TContext context)
        {
            if (node.Interfaces?.Count > 0)
            {
                context.Writer.Write(" implements ");

                for (int i = 0; i < node.Interfaces.Count; ++i)
                {
                    Visit(node.Interfaces[i], context);
                    if (i < node.Interfaces.Count - 1)
                        context.Writer.Write(" & ");
                }
            }
        }

        //TODO: remove
        private void Visit<T>(List<T>? nodes, TContext context, string delimiter, string start)
            where T : ASTNode
        {
            if (nodes != null)
            {
                if (!string.IsNullOrEmpty(start))
                    context.Writer.Write(start);

                for (int i = 0; i < nodes.Count; ++i)
                {
                    Visit(nodes[i], context);
                    if (i < nodes.Count - 1)
                        context.Writer.Write(delimiter);
                }
            }
        }

        private void VisitDirectives(IHasDirectivesNode node, TContext context)
        {
            if (node.Directives?.Count > 0)
            {
                context.Writer.Write(' ');

                for (int i = 0; i < node.Directives.Count; ++i)
                {
                    Visit(node.Directives[i], context);
                    if (i < node.Directives.Count - 1)
                        context.Writer.Write(' ');
                }
            }
        }

        private string GetOperationType(OperationType type) => type switch
        {
            OperationType.Mutation => "mutation",
            OperationType.Subscription => "subscription",
            _ => "query",
        };

        private void WriteIndent(TContext context, int level)
        {
            for (int i = 0; i < level; ++i)
                context.Writer.Write("  ");
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

        private void Write(TContext context, ROM value)
        {
#if NETSTANDARD2_0
            context.Writer.Write(value.ToString());
#elif NETSTANDARD2_1_OR_GREATER
            context.Writer.Write(value.Span);
#endif
        }
    }
}
