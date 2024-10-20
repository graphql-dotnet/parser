using System.Diagnostics;
using GraphQLParser.AST;

namespace GraphQLParser;

// WARNING: mutable ref struct, pass it by reference to those methods that will change it
internal ref partial struct ParserContext
{
    // http://spec.graphql.org/October2021/#Document
    public GraphQLDocument ParseDocument()
    {
        int start = _currentToken.Start;
        var definitions = ParseDefinitionsIfNotEOF();

        SetCurrentComments(null); // push current (last) comment into _unattachedComments

        _document.Location = new GraphQLLocation
        (
            start,
            // Formally, to denote the end of the document, it is better to use _prevToken.End,
            // since _prevToken represents some real meaningful token; _currentToken here is always EOF.
            // EOF is a technical token with length = 0, _prevToken.End and _currentToken.End have the same value here.
            _prevToken.End // equals to _currentToken.End (EOF)
        );
        _document.Definitions = definitions;
        _document.UnattachedComments = _unattachedComments;

        Debug.Assert(_currentDepth == 1, "Depth has not returned to 1 after parsing document");

        return _document;
    }

    // http://spec.graphql.org/October2021/#TypeCondition
    private GraphQLTypeCondition? ParseTypeCondition(bool optional)
    {
        if (optional && _currentToken.Value != "on")
            return null;

        IncreaseDepth();

        int start = _currentToken.Start;

        var condition = NodeHelper.CreateGraphQLTypeCondition(_ignoreOptions);

        condition.Comments = GetComments();
        ExpectKeyword("on");
        condition.Type = ParseNamedType();
        condition.Location = GetLocation(start);

        DecreaseDepth();
        return condition;
    }

    // http://spec.graphql.org/October2021/#Argument
    public GraphQLArgument ParseArgument()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var arg = NodeHelper.CreateGraphQLArgument(_ignoreOptions);

        arg.Comments = GetComments();
        arg.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#Argument");
        Expect(TokenKind.COLON);
        arg.Value = ParseValueLiteral(false);
        arg.Location = GetLocation(start);

        DecreaseDepth();
        return arg;
    }

    // http://spec.graphql.org/October2021/#Arguments
    public GraphQLArguments ParseArguments()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var args = NodeHelper.CreateGraphQLArguments(_ignoreOptions);

        args.Comments = GetComments();
        args.Items = OneOrMore(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseArgument(), TokenKind.PAREN_R);
        args.Location = GetLocation(start);

        DecreaseDepth();
        return args;
    }

    // http://spec.graphql.org/October2021/#ArgumentsDefinition
    public GraphQLArgumentsDefinition ParseArgumentsDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var argsDef = NodeHelper.CreateGraphQLArgumentsDefinition(_ignoreOptions);

        argsDef.Comments = GetComments();
        argsDef.Items = OneOrMore(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseInputValueDefinition(true), TokenKind.PAREN_R);
        argsDef.Location = GetLocation(start);

        DecreaseDepth();
        return argsDef;
    }

    // http://spec.graphql.org/October2021/#InputFieldsDefinition
    public GraphQLInputFieldsDefinition ParseInputFieldsDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var inputFieldsDef = NodeHelper.CreateGraphQLInputFieldsDefinition(_ignoreOptions);

        inputFieldsDef.Comments = GetComments();
        inputFieldsDef.Items = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseInputValueDefinition(false), TokenKind.BRACE_R);
        inputFieldsDef.Location = GetLocation(start);

        DecreaseDepth();
        return inputFieldsDef;
    }

    // http://spec.graphql.org/October2021/#FieldsDefinition
    public GraphQLFieldsDefinition ParseFieldsDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var fieldsDef = NodeHelper.CreateGraphQLFieldsDefinition(_ignoreOptions);

        fieldsDef.Comments = GetComments();
        fieldsDef.Items = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseFieldDefinition(), TokenKind.BRACE_R);
        fieldsDef.Location = GetLocation(start);

        DecreaseDepth();
        return fieldsDef;
    }

    // http://spec.graphql.org/October2021/#EnumValuesDefinition
    public GraphQLEnumValuesDefinition ParseEnumValuesDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var enumValuesDef = NodeHelper.CreateGraphQLEnumValuesDefinition(_ignoreOptions);

        enumValuesDef.Comments = GetComments();
        enumValuesDef.Items = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseEnumValueDefinition(), TokenKind.BRACE_R);
        enumValuesDef.Location = GetLocation(start);

        DecreaseDepth();
        return enumValuesDef;
    }

    // http://spec.graphql.org/October2021/#VariableDefinitions
    public GraphQLVariablesDefinition ParseVariablesDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var variablesDef = NodeHelper.CreateGraphQLVariablesDefinition(_ignoreOptions);

        variablesDef.Comments = GetComments();
        variablesDef.Items = OneOrMore(TokenKind.PAREN_L, (ref ParserContext context) => context.ParseVariableDefinition(), TokenKind.PAREN_R);
        variablesDef.Location = GetLocation(start);

        DecreaseDepth();
        return variablesDef;
    }

    // http://spec.graphql.org/October2021/#BooleanValue
    // There is no true/false value check, see calling method.
    private GraphQLBooleanValue ParseBooleanValue(bool value)
    {
        IncreaseDepth();

        var token = _currentToken;

        var val = NodeHelper.CreateGraphQLBooleanValue(_ignoreOptions, value);

        val.Comments = GetComments();
        Advance();
        val.Location = GetLocation(token.Start);

        DecreaseDepth();
        return val;
    }

    private ASTNode ParseDefinition()
    {
        if (Peek(TokenKind.BRACE_L))
            return ParseOperationDefinition();

        if (Peek(TokenKind.NAME))
            return ParseNamedDefinition();

        if (Peek(TokenKind.STRING))
            return ParseNamedDefinitionWithDescription();

        return Throw_Unexpected_Token();
    }

    private List<ASTNode> ParseDefinitionsIfNotEOF()
    {
        var result = new List<ASTNode>();

        if (_currentToken.Kind != TokenKind.EOF)
        {
            do
            {
                result.Add(ParseDefinition());
            }
            while (!Skip(TokenKind.EOF));
        }

        return result;
    }

    // http://spec.graphql.org/October2021/#Comment
    // NOTE: method parses zero or more comments into list, they are not merged into one comment!
    private void ParseComments()
    {
        // skip comments
        if (_ignoreOptions.HasFlag(IgnoreOptions.Comments))
        {
            while (Peek(TokenKind.COMMENT))
            {
                Advance(fromParseComment: true);
            }
            return;
        }

        if (!Peek(TokenKind.COMMENT))
        {
            return;
        }

        IncreaseDepth();

        var comments = new List<GraphQLComment>();

        do
        {
            var comment = NodeHelper.CreateGraphQLComment(_ignoreOptions, _currentToken.Value);
            comment.Location = new GraphQLLocation(_currentToken.Start, _currentToken.End);
            comments.Add(comment);
            Advance(fromParseComment: true);
        }
        while (_currentToken.Kind == TokenKind.COMMENT);

        SetCurrentComments(comments);
        DecreaseDepth();
    }

    private void SetCurrentComments(List<GraphQLComment>? comments)
    {
        if (_currentComments != null)
            (_unattachedComments ??= new List<List<GraphQLComment>>()).Add(_currentComments);

        _currentComments = comments;
    }

    private List<GraphQLComment>? GetComments()
    {
        var ret = _currentComments;
        _currentComments = null;
        return ret;
    }

    // http://spec.graphql.org/October2021/#Directive
    public GraphQLDirective ParseDirective()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var dir = NodeHelper.CreateGraphQLDirective(_ignoreOptions);

        dir.Comments = GetComments();
        Expect(TokenKind.AT);
        dir.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#Directive");
        dir.Arguments = Peek(TokenKind.PAREN_L) ? ParseArguments() : null;
        dir.Location = GetLocation(start);

        DecreaseDepth();
        return dir;
    }

    // http://spec.graphql.org/October2021/#DirectiveDefinition
    public GraphQLDirectiveDefinition ParseDirectiveDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLDirectiveDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        ExpectKeyword("directive");
        Expect(TokenKind.AT);
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#DirectiveDefinition");
        def.Arguments = Peek(TokenKind.PAREN_L) ? ParseArgumentsDefinition() : null;
        def.Repeatable = Peek(TokenKind.NAME) && ParseRepeatable();
        ExpectKeyword("on");
        def.Locations = ParseDirectiveLocations();
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    private bool ParseRepeatable()
    {
        if (_currentToken.Value == "on")
            return false;

        if (_currentToken.Value == "repeatable")
        {
            Advance();
            return true;
        }

        Throw_Unexpected_Token("; did you miss 'repeatable'?");
        return false; // for compiler
    }

    private GraphQLDirectiveLocations ParseDirectiveLocations()
    {
        IncreaseDepth();
        var comments = GetComments();

        int start = _currentToken.Start;

        var directiveLocations = NodeHelper.CreateGraphQLDirectiveLocations(_ignoreOptions);

        var items = new List<DirectiveLocation>();

        // Directive locations may be defined with an optional leading | character
        // to aid formatting when representing a longer list of possible locations
        _ = Skip(TokenKind.PIPE);

        do
        {
            items.Add(ParseDirectiveLocation());
        }
        while (Skip(TokenKind.PIPE));

        directiveLocations.Items = items;
        directiveLocations.Comments = comments;
        directiveLocations.Location = GetLocation(start);

        DecreaseDepth();
        return directiveLocations;
    }

    // http://spec.graphql.org/October2021/#Directives
    public GraphQLDirectives ParseDirectives()
    {
        IncreaseDepth();
        // Directives go one after another without any "list prefix", so it is impossible
        // to distinguish the comment of the first directive from the comment to the entire
        // list of directives. Therefore, a comment for the directive itself is used.
        //var comments = GetComments();

        int start = _currentToken.Start;

        var directives = NodeHelper.CreateGraphQLDirectives(_ignoreOptions);

        // OneOrMore does not work here because there are no open and close tokens
        var items = new List<GraphQLDirective> { ParseDirective() };

        while (Peek(TokenKind.AT))
            items.Add(ParseDirective());

        directives.Items = items;
        //directives.Comment = comment;
        directives.Location = GetLocation(start);

        DecreaseDepth();
        return directives;
    }

    // http://spec.graphql.org/October2021/#EnumTypeDefinition
    public GraphQLEnumTypeDefinition ParseEnumTypeDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLEnumTypeDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        ExpectKeyword("enum");
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#EnumTypeDefinition");
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Values = Peek(TokenKind.BRACE_L) ? ParseEnumValuesDefinition() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#EnumTypeExtension
    // Note that due to the spec type extensions have no descriptions.
    private GraphQLEnumTypeExtension ParseEnumTypeExtension(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        ExpectKeyword("enum");

        var extension = NodeHelper.CreateGraphQLEnumTypeExtension(_ignoreOptions);

        extension.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#EnumTypeExtension");
        extension.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        extension.Values = Peek(TokenKind.BRACE_L) ? ParseEnumValuesDefinition() : null;
        extension.Comments = comments;
        extension.Location = GetLocation(start);

        if (extension.Directives == null && extension.Values == null)
            return (GraphQLEnumTypeExtension)Throw_Unexpected_Token("; for more information see http://spec.graphql.org/October2021/#EnumTypeExtension");

        DecreaseDepth();
        return extension;
    }

    // http://spec.graphql.org/October2021/#EnumValue
    private GraphQLEnumValue ParseEnumValue(bool validate)
    {
        if (validate && (_currentToken.Value == "true" || _currentToken.Value == "false" || _currentToken.Value == "null"))
        {
            Throw_Unexpected_Token("; enum values are represented as unquoted names but not 'true' or 'false' or 'null'.");
        }

        IncreaseDepth();

        int start = _currentToken.Start;

        var enumVal = NodeHelper.CreateGraphQLEnumValue(_ignoreOptions);

        enumVal.Comments = GetComments();
        enumVal.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#EnumValue");
        enumVal.Location = GetLocation(start);

        DecreaseDepth();
        return enumVal;
    }

    // http://spec.graphql.org/October2021/#EnumValueDefinition
    public GraphQLEnumValueDefinition ParseEnumValueDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLEnumValueDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        def.EnumValue = ParseEnumValue(true);
        def.Name = def.EnumValue.Name; // ATTENTION: should set Name property (inherited from GraphQLTypeDefinition)
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#FieldDefinition
    public GraphQLFieldDefinition ParseFieldDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLFieldDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#FieldDefinition");
        def.Arguments = Peek(TokenKind.PAREN_L) ? ParseArgumentsDefinition() : null;
        Expect(TokenKind.COLON);
        def.Type = ParseType();
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#Field
    // http://spec.graphql.org/October2021/#Alias
    public GraphQLField ParseField()
    {
        IncreaseDepth();

        // start of alias (if exists) equals start of field
        int start = _currentToken.Start;

        var comments = GetComments(); // Greedy parsing for comment here - we read comment for field itself, not for alias.
        var nameOrAlias = ParseName("; for more information see http://spec.graphql.org/October2021/#Field");

        GraphQLName name;
        GraphQLName? alias;

        //GraphQLComment? nameComment;
        //GraphQLComment? aliasComment;

        GraphQLLocation aliasLocation = default;

        if (Skip(TokenKind.COLON)) // alias exists
        {
            aliasLocation = GetLocation(start);

            //nameComment = GetComment();
            //aliasComment = nameOrAliasComment;

            name = ParseName("; for more information see http://spec.graphql.org/October2021/#Field");
            alias = nameOrAlias;
        }
        else // no alias
        {
            //aliasComment = null;
            //nameComment = nameOrAliasComment;

            alias = null;
            name = nameOrAlias;
        }

        var field = NodeHelper.CreateGraphQLField(_ignoreOptions);

        if (alias is not null)
        {
            var aliasNode = NodeHelper.CreateGraphQLAlias(_ignoreOptions);

            //aliasNode.Comment = aliasComment; // Alias can not have comment
            aliasNode.Name = alias;
            aliasNode.Location = aliasLocation;

            field.Alias = aliasNode;
        }
        field.Comments = comments;
        field.Name = name;
        field.Arguments = Peek(TokenKind.PAREN_L) ? ParseArguments() : null;
        field.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        field.SelectionSet = Peek(TokenKind.BRACE_L) ? ParseSelectionSet() : null;
        field.Location = GetLocation(start);

        DecreaseDepth();
        return field;
    }

    // http://spec.graphql.org/October2021/#FloatValue
    private GraphQLFloatValue ParseFloatValue()
    {
        IncreaseDepth();

        var token = _currentToken;

        var val = NodeHelper.CreateGraphQLFloatValue(_ignoreOptions, token.Value);

        val.Comments = GetComments();
        Advance();
        val.Location = GetLocation(token.Start);

        DecreaseDepth();
        return val;
    }

    private ASTNode ParseFragment()
    {
        int start = _currentToken.Start;
        var comments = GetComments();
        Expect(TokenKind.SPREAD);

        return Peek(TokenKind.NAME) && _currentToken.Value != "on"
            ? ParseFragmentSpread(start, comments)
            : ParseInlineFragment(start, comments);
    }

    // http://spec.graphql.org/October2021/#FragmentSpread
    private GraphQLFragmentSpread ParseFragmentSpread(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        var spread = NodeHelper.CreateGraphQLFragmentSpread(_ignoreOptions);

        spread.FragmentName = ParseFragmentName();
        spread.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        spread.Comments = comments;
        spread.Location = GetLocation(start);

        DecreaseDepth();
        return spread;
    }

    // http://spec.graphql.org/October2021/#InlineFragment
    private GraphQLInlineFragment ParseInlineFragment(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        var frag = NodeHelper.CreateGraphQLInlineFragment(_ignoreOptions);

        frag.TypeCondition = ParseTypeCondition(optional: true);
        frag.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        frag.SelectionSet = ParseSelectionSet();
        frag.Comments = comments;
        frag.Location = GetLocation(start);

        DecreaseDepth();
        return frag;
    }

    // http://spec.graphql.org/October2021/#FragmentDefinition
    public GraphQLFragmentDefinition ParseFragmentDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLFragmentDefinition(_ignoreOptions);

        def.Comments = GetComments();
        ExpectKeyword("fragment");
        def.FragmentName = ParseFragmentName();
        def.TypeCondition = ParseTypeCondition(optional: false)!; // never returns null
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.SelectionSet = ParseSelectionSet();
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#FragmentName
    private GraphQLFragmentName ParseFragmentName()
    {
        if (_currentToken.Value == "on")
        {
            Throw_Unexpected_Token("; fragment name can not be 'on'.");
        }

        IncreaseDepth();

        int start = _currentToken.Start;

        var fragName = NodeHelper.CreateGraphQLFragmentName(_ignoreOptions);

        fragName.Comments = GetComments();
        fragName.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#FragmentName");
        fragName.Location = GetLocation(start);

        DecreaseDepth();
        return fragName;
    }

    // http://spec.graphql.org/October2021/#ImplementsInterfaces
    private GraphQLImplementsInterfaces ParseImplementsInterfaces()
    {
        IncreaseDepth();
        var comments = GetComments();

        int start = _currentToken.Start;

        ExpectKeyword("implements");

        var implementsInterfaces = NodeHelper.CreateGraphQLImplementsInterfaces(_ignoreOptions);

        List<GraphQLNamedType> types = new();

        // Objects that implement interfaces may be defined with an optional leading & character
        // to aid formatting when representing a longer list of implemented interfaces
        _ = Skip(TokenKind.AMPERSAND);

        do
        {
            types.Add(ParseNamedType());
        }
        while (Skip(TokenKind.AMPERSAND));

        implementsInterfaces.Items = types;
        implementsInterfaces.Comments = comments;
        implementsInterfaces.Location = GetLocation(start);

        DecreaseDepth();
        return implementsInterfaces;
    }

    // http://spec.graphql.org/October2021/#InputObjectTypeDefinition
    public GraphQLInputObjectTypeDefinition ParseInputObjectTypeDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLInputObjectTypeDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        ExpectKeyword("input");
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#InputObjectTypeDefinition");
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Fields = Peek(TokenKind.BRACE_L) ? ParseInputFieldsDefinition() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#InputObjectTypeExtension
    // Note that due to the spec type extensions have no descriptions.
    private GraphQLInputObjectTypeExtension ParseInputObjectTypeExtension(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        var extension = NodeHelper.CreateGraphQLInputObjectTypeExtension(_ignoreOptions);

        extension.Comments = comments;
        ExpectKeyword("input");
        extension.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#InputObjectTypeExtension");
        extension.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        extension.Fields = Peek(TokenKind.BRACE_L) ? ParseInputFieldsDefinition() : null;
        extension.Location = GetLocation(start);

        if (extension.Directives == null && extension.Fields == null)
            return (GraphQLInputObjectTypeExtension)Throw_Unexpected_Token("; for more information see http://spec.graphql.org/October2021/#InputObjectTypeExtension");

        DecreaseDepth();
        return extension;
    }

    // http://spec.graphql.org/October2021/#InputValueDefinition
    public GraphQLInputValueDefinition ParseInputValueDefinition(bool? argument)
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLInputValueDefinition(_ignoreOptions, argument);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#InputValueDefinition");
        Expect(TokenKind.COLON);
        def.Type = ParseType();
        def.DefaultValue = Skip(TokenKind.EQUALS) ? ParseValueLiteral(true) : null;
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#IntValue
    private GraphQLIntValue ParseIntValue()
    {
        IncreaseDepth();

        var token = _currentToken;

        var val = NodeHelper.CreateGraphQLIntValue(_ignoreOptions, token.Value);

        val.Comments = GetComments();
        Advance();
        val.Location = GetLocation(token.Start);

        DecreaseDepth();
        return val;
    }

    // http://spec.graphql.org/October2021/#InterfaceTypeDefinition
    public GraphQLInterfaceTypeDefinition ParseInterfaceTypeDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLInterfaceTypeDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        ExpectKeyword("interface");
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#InterfaceTypeDefinition");
        def.Interfaces = _currentToken.Value == "implements" ? ParseImplementsInterfaces() : null;
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Fields = Peek(TokenKind.BRACE_L) ? ParseFieldsDefinition() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#InterfaceTypeExtension
    // Note that due to the spec type extensions have no descriptions.
    private GraphQLInterfaceTypeExtension ParseInterfaceTypeExtension(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        var extension = NodeHelper.CreateGraphQLInterfaceTypeExtension(_ignoreOptions);

        extension.Comments = comments;
        ExpectKeyword("interface");
        extension.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#InterfaceTypeExtension");
        extension.Interfaces = _currentToken.Value == "implements" ? ParseImplementsInterfaces() : null;
        extension.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        extension.Fields = Peek(TokenKind.BRACE_L) ? ParseFieldsDefinition() : null;
        extension.Location = GetLocation(start);

        if (extension.Directives == null && extension.Fields == null && extension.Interfaces == null)
            return (GraphQLInterfaceTypeExtension)Throw_Unexpected_Token("; for more information see http://spec.graphql.org/October2021/#InterfaceTypeExtension");

        DecreaseDepth();
        return extension;
    }

    // http://spec.graphql.org/October2021/#ListValue
    private GraphQLListValue ParseListValue(bool isConstant)
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        // the compiler caches these delegates in the generated code
        ParseCallback<GraphQLValue> constant = (ref ParserContext context) => context.ParseValueLiteral(true);
        ParseCallback<GraphQLValue> value = (ref ParserContext context) => context.ParseValueLiteral(false);

        var val = NodeHelper.CreateGraphQLListValue(_ignoreOptions);

        val.Comments = GetComments();
        val.Values = ZeroOrMore(TokenKind.BRACKET_L, isConstant ? constant : value, TokenKind.BRACKET_R);
        val.Location = GetLocation(start);

        DecreaseDepth();
        return val;
    }

    // http://spec.graphql.org/October2021/#Name
    private GraphQLName ParseName(string description)
    {
        IncreaseDepth();

        int start = _currentToken.Start;
        var value = _currentToken.Value;

        var name = NodeHelper.CreateGraphQLName(_ignoreOptions, value);

        name.Comments = GetComments();
        Expect(TokenKind.NAME, description);
        name.Location = GetLocation(start);

        DecreaseDepth();
        return name;
    }

    internal ASTNode ParseNamedDefinition(string[]? oneOf = null) // internal for tests
    {
        var keyword = ExpectOneOf(oneOf ?? TopLevelKeywordOneOf, advance: false);
        return keyword switch
        {
            "query" => ParseOperationDefinition(),
            "mutation" => ParseOperationDefinition(),
            "subscription" => ParseOperationDefinition(),
            "fragment" => ParseFragmentDefinition(),
            "schema" => ParseSchemaDefinition(),
            "scalar" => ParseScalarTypeDefinition(),
            "type" => ParseObjectTypeDefinition(),
            "interface" => ParseInterfaceTypeDefinition(),
            "union" => ParseUnionTypeDefinition(),
            "enum" => ParseEnumTypeDefinition(),
            "input" => ParseInputObjectTypeDefinition(),
            "extend" => ParseTypeSystemExtension(),
            "directive" => ParseDirectiveDefinition(),

            _ => throw new NotSupportedException($"Unexpected keyword '{keyword}' in {nameof(ParseNamedDefinition)}.")
        };
    }

    // TODO: 1. May be changed to use ExpectOneOf, or
    // TODO: 2. https://github.com/graphql/graphql-spec/pull/892 which allow to remove this method
    private ASTNode ParseNamedDefinitionWithDescription()
    {
        // look-ahead to next token (_currentToken remains unchanged)
        var token = Lexer.Lex(_source, _currentToken.End);
        // skip comments
        while (token.Kind != TokenKind.EOF && token.Kind == TokenKind.COMMENT)
        {
            token = Lexer.Lex(_source, token.End);
        }

        // verify this is a NAME
        if (token.Kind != TokenKind.NAME)
            return Throw_Unexpected_Token();

        return token.Value.Span switch
        {
            "schema" => ParseSchemaDefinition(),
            "scalar" => ParseScalarTypeDefinition(),
            "type" => ParseObjectTypeDefinition(),
            "interface" => ParseInterfaceTypeDefinition(),
            "union" => ParseUnionTypeDefinition(),
            "enum" => ParseEnumTypeDefinition(),
            "input" => ParseInputObjectTypeDefinition(),
            "directive" => ParseDirectiveDefinition(),
            _ => Throw_Unexpected_Token()
        };
    }

    // http://spec.graphql.org/October2021/#NamedType
    private GraphQLNamedType ParseNamedType()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var named = NodeHelper.CreateGraphQLNamedType(_ignoreOptions);

        named.Comments = GetComments();
        named.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#NamedType");
        named.Location = GetLocation(start);

        DecreaseDepth();
        return named;
    }

    private GraphQLValue ParseNameValue()
    {
        return _currentToken.Value.Span switch
        {
            "true" => ParseBooleanValue(true),
            "false" => ParseBooleanValue(false),
            "null" => ParseNullValue(),
            _ => ParseEnumValue(false)
        };
    }

    // http://spec.graphql.org/October2021/#ObjectValue
    private GraphQLObjectValue ParseObjectValue(bool isConstant)
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        // the compiler caches these delegates in the generated code
        ParseCallback<GraphQLObjectField> constant = (ref ParserContext context) => context.ParseObjectField(true);
        ParseCallback<GraphQLObjectField> value = (ref ParserContext context) => context.ParseObjectField(false);

        var val = NodeHelper.CreateGraphQLObjectValue(_ignoreOptions);

        val.Comments = GetComments();
        val.Fields = ZeroOrMore(TokenKind.BRACE_L, isConstant ? constant : value, TokenKind.BRACE_R);
        val.Location = GetLocation(start);

        DecreaseDepth();
        return val;
    }

    // http://spec.graphql.org/October2021/#NullValue
    private GraphQLNullValue ParseNullValue()
    {
        IncreaseDepth();

        var token = _currentToken;

        var val = NodeHelper.CreateGraphQLNullValue(_ignoreOptions);

        val.Comments = GetComments();
        Advance();
        val.Location = GetLocation(token.Start);

        DecreaseDepth();
        return val;
    }

    // http://spec.graphql.org/October2021/#ObjectField
    private GraphQLObjectField ParseObjectField(bool isConstant)
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var field = NodeHelper.CreateGraphQLObjectField(_ignoreOptions);

        field.Comments = GetComments();
        field.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#ObjectField");
        Expect(TokenKind.COLON);
        field.Value = ParseValueLiteral(isConstant);
        field.Location = GetLocation(start);

        DecreaseDepth();
        return field;
    }

    // http://spec.graphql.org/October2021/#ObjectTypeDefinition
    public GraphQLObjectTypeDefinition ParseObjectTypeDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLObjectTypeDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        ExpectKeyword("type");
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#ObjectTypeDefinition");
        def.Interfaces = _currentToken.Value == "implements" ? ParseImplementsInterfaces() : null;
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Fields = Peek(TokenKind.BRACE_L) ? ParseFieldsDefinition() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#ObjectTypeExtension
    // Note that due to the spec type extensions have no descriptions.
    private GraphQLObjectTypeExtension ParseObjectTypeExtension(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        var extension = NodeHelper.CreateGraphQLObjectTypeExtension(_ignoreOptions);

        extension.Comments = comments;
        ExpectKeyword("type");
        extension.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#ObjectTypeExtension");
        extension.Interfaces = _currentToken.Value == "implements" ? ParseImplementsInterfaces() : null;
        extension.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        extension.Fields = Peek(TokenKind.BRACE_L) ? ParseFieldsDefinition() : null;
        extension.Location = GetLocation(start);

        if (extension.Directives == null && extension.Fields == null && extension.Interfaces == null)
            return (GraphQLObjectTypeExtension)Throw_Unexpected_Token("; for more information see http://spec.graphql.org/October2021/#ObjectTypeExtension");

        DecreaseDepth();
        return extension;
    }

    // http://spec.graphql.org/October2021/#OperationDefinition
    public ASTNode ParseOperationDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLOperationDefinition(_ignoreOptions);

        def.Comments = GetComments();

        if (Peek(TokenKind.BRACE_L))
        {
            def.Operation = OperationType.Query;
            def.SelectionSet = ParseSelectionSet();
            def.Location = GetLocation(start);
        }
        else
        {
            def.Operation = ParseOperationType();
            def.Name = Peek(TokenKind.NAME) ? ParseName("; for more information see http://spec.graphql.org/October2021/#OperationDefinition") : null; // Peek(TokenKind.NAME) because of anonymous query
            def.Variables = Peek(TokenKind.PAREN_L) ? ParseVariablesDefinition() : null;
            def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
            def.SelectionSet = ParseSelectionSet();
            def.Location = GetLocation(start);
        }

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#OperationType
    internal OperationType ParseOperationType(string[]? oneOf = null) // internal for tests
    {
        var keyword = ExpectOneOf(oneOf ?? OperationTypeOneOf);
        return keyword switch
        {
            "query" => OperationType.Query,
            "mutation" => OperationType.Mutation,
            "subscription" => OperationType.Subscription,

            _ => throw new NotSupportedException($"Unexpected keyword '{keyword}' in {nameof(ParseOperationType)}.")
        };
    }

    // http://spec.graphql.org/October2021/#DirectiveLocation
    internal DirectiveLocation ParseDirectiveLocation(string[]? oneOf = null) // internal for tests
    {
        var keyword = ExpectOneOf(oneOf ?? DirectiveLocationOneOf);
        return keyword switch
        {
            // http://spec.graphql.org/October2021/#ExecutableDirectiveLocation
            "QUERY" => DirectiveLocation.Query,
            "MUTATION" => DirectiveLocation.Mutation,
            "SUBSCRIPTION" => DirectiveLocation.Subscription,
            "FIELD" => DirectiveLocation.Field,
            "FRAGMENT_DEFINITION" => DirectiveLocation.FragmentDefinition,
            "FRAGMENT_SPREAD" => DirectiveLocation.FragmentSpread,
            "INLINE_FRAGMENT" => DirectiveLocation.InlineFragment,
            "VARIABLE_DEFINITION" => DirectiveLocation.VariableDefinition,

            // http://spec.graphql.org/October2021/#TypeSystemDirectiveLocation
            "SCHEMA" => DirectiveLocation.Schema,
            "SCALAR" => DirectiveLocation.Scalar,
            "OBJECT" => DirectiveLocation.Object,
            "FIELD_DEFINITION" => DirectiveLocation.FieldDefinition,
            "ARGUMENT_DEFINITION" => DirectiveLocation.ArgumentDefinition,
            "INTERFACE" => DirectiveLocation.Interface,
            "UNION" => DirectiveLocation.Union,
            "ENUM" => DirectiveLocation.Enum,
            "ENUM_VALUE" => DirectiveLocation.EnumValue,
            "INPUT_OBJECT" => DirectiveLocation.InputObject,
            "INPUT_FIELD_DEFINITION" => DirectiveLocation.InputFieldDefinition,

            _ => throw new NotSupportedException($"Unexpected keyword '{keyword}' in {nameof(ParseDirectiveLocation)}.")
        };
    }

    // http://spec.graphql.org/October2021/#RootOperationTypeDefinition
    public GraphQLRootOperationTypeDefinition ParseRootOperationTypeDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLOperationTypeDefinition(_ignoreOptions);

        def.Comments = GetComments();
        def.Operation = ParseOperationType();
        Expect(TokenKind.COLON);
        def.Type = ParseNamedType();
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#ScalarTypeDefinition
    public GraphQLScalarTypeDefinition ParseScalarTypeDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLScalarTypeDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        ExpectKeyword("scalar");
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#ScalarTypeDefinition");
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#SchemaExtension
    // Note that due to the spec schema extensions have no descriptions.
    private GraphQLSchemaExtension ParseSchemaExtension(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        var extension = NodeHelper.CreateGraphQLSchemaExtension(_ignoreOptions);

        extension.Comments = comments;
        ExpectKeyword("schema");
        extension.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        extension.OperationTypes = Peek(TokenKind.BRACE_L) ? OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseRootOperationTypeDefinition(), TokenKind.BRACE_R) : null;
        extension.Location = GetLocation(start);

        if (extension.Directives == null && extension.OperationTypes == null)
            return (GraphQLSchemaExtension)Throw_Unexpected_Token("; for more information see http://spec.graphql.org/October2021/#SchemaExtension");

        DecreaseDepth();
        return extension;
    }

    // http://spec.graphql.org/October2021/#ScalarTypeExtension
    // Note that due to the spec type extensions have no descriptions.
    private GraphQLScalarTypeExtension ParseScalarTypeExtension(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        var extension = NodeHelper.CreateGraphQLScalarTypeExtension(_ignoreOptions);

        extension.Comments = comments;
        ExpectKeyword("scalar");
        extension.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#ScalarTypeExtension");
        extension.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        extension.Location = GetLocation(start);

        if (extension.Directives == null)
            return (GraphQLScalarTypeExtension)Throw_Unexpected_Token("; for more information see http://spec.graphql.org/October2021/#ScalarTypeExtension");

        DecreaseDepth();
        return extension;
    }

    // http://spec.graphql.org/October2021/#SchemaDefinition
    public GraphQLSchemaDefinition ParseSchemaDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLSchemaDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        ExpectKeyword("schema");
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.OperationTypes = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseRootOperationTypeDefinition(), TokenKind.BRACE_R);
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#Selection
    private ASTNode ParseSelection()
    {
        return Peek(TokenKind.SPREAD) ?
            ParseFragment() :
            ParseField();
    }

    // http://spec.graphql.org/October2021/#SelectionSet
    public GraphQLSelectionSet ParseSelectionSet()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var selection = NodeHelper.CreateGraphQLSelectionSet(_ignoreOptions);

        selection.Comments = GetComments();
        selection.Selections = OneOrMore(TokenKind.BRACE_L, (ref ParserContext context) => context.ParseSelection(), TokenKind.BRACE_R);
        selection.Location = GetLocation(start);

        DecreaseDepth();
        return selection;
    }

    // http://spec.graphql.org/October2021/#StringValue
    private GraphQLStringValue ParseStringValue()
    {
        IncreaseDepth();

        var token = _currentToken;

        var val = NodeHelper.CreateGraphQLStringValue(_ignoreOptions, token.Value);

        val.Comments = GetComments();
        Advance();
        val.Location = GetLocation(token.Start);

        DecreaseDepth();
        return val;
    }

    // http://spec.graphql.org/October2021/#Description
    public GraphQLDescription ParseDescription()
    {
        IncreaseDepth();

        var token = _currentToken;
        Advance();

        var descr = NodeHelper.CreateGraphQLDescription(_ignoreOptions, token.Value);

        descr.Location = GetLocation(token.Start);

        DecreaseDepth();
        return descr;
    }

    // http://spec.graphql.org/October2021/#Type
    private GraphQLType ParseType()
    {
        IncreaseDepth();

        GraphQLType type;
        int start = _currentToken.Start;
        if (Peek(TokenKind.BRACKET_L))
        {
            var listType = NodeHelper.CreateGraphQLListType(_ignoreOptions);

            listType.Comments = GetComments();

            Advance(); // skip BRACKET_L

            listType.Type = ParseType();

            Expect(TokenKind.BRACKET_R);

            listType.Location = GetLocation(start);
            type = listType;
        }
        else
        {
            type = ParseNamedType();
        }

        if (Skip(TokenKind.BANG))
        {
            var nonNull = NodeHelper.CreateGraphQLNonNullType(_ignoreOptions);

            nonNull.Type = type;
            // move comment from wrapped type to wrapping type
            nonNull.Comments = type.Comments;
            type.Comments = null;
            nonNull.Location = GetLocation(start);
            type = nonNull;
        }

        DecreaseDepth();
        return type;
    }

    // http://spec.graphql.org/October2021/#TypeSystemExtension : SchemaExtension / TypeExtension
    public ASTNode ParseTypeSystemExtension(string[]? oneOf = null)
    {
        int start = _currentToken.Start;
        var comments = GetComments();

        ExpectKeyword("extend");

        var keyword = ExpectOneOf(oneOf ?? TypeExtensionOneOf, advance: false);
        return keyword switch
        {
            "schema" => ParseSchemaExtension(start, comments),
            "scalar" => ParseScalarTypeExtension(start, comments),
            "type" => ParseObjectTypeExtension(start, comments),
            "interface" => ParseInterfaceTypeExtension(start, comments),
            "union" => ParseUnionTypeExtension(start, comments),
            "enum" => ParseEnumTypeExtension(start, comments),
            "input" => ParseInputObjectTypeExtension(start, comments),

            _ => throw new NotSupportedException($"Unexpected keyword '{keyword}' in {nameof(ParseTypeSystemExtension)}.")
        };
    }

    // http://spec.graphql.org/October2021/#UnionMemberTypes
    private GraphQLUnionMemberTypes ParseUnionMemberTypes()
    {
        IncreaseDepth();
        var comments = GetComments();

        int start = _currentToken.Start;

        Expect(TokenKind.EQUALS);

        var unionMemberTypes = NodeHelper.CreateGraphQLUnionMemberTypes(_ignoreOptions);

        List<GraphQLNamedType> types = new();

        // Union members may be defined with an optional leading | character
        // to aid formatting when representing a longer list of possible types
        _ = Skip(TokenKind.PIPE);

        do
        {
            types.Add(ParseNamedType());
        }
        while (Skip(TokenKind.PIPE));

        unionMemberTypes.Items = types;
        unionMemberTypes.Comments = comments;
        unionMemberTypes.Location = GetLocation(start);

        DecreaseDepth();
        return unionMemberTypes;
    }

    // http://spec.graphql.org/October2021/#UnionTypeDefinition
    public GraphQLUnionTypeDefinition ParseUnionTypeDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLUnionTypeDefinition(_ignoreOptions);

        def.Description = Peek(TokenKind.STRING) ? ParseDescription() : null;
        def.Comments = GetComments();
        ExpectKeyword("union");
        def.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#UnionTypeDefinition");
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Types = Peek(TokenKind.EQUALS) ? ParseUnionMemberTypes() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }

    // http://spec.graphql.org/October2021/#UnionTypeExtension
    // Note that due to the spec type extensions have no descriptions.
    private GraphQLUnionTypeExtension ParseUnionTypeExtension(int start, List<GraphQLComment>? comments)
    {
        IncreaseDepth();

        var extension = NodeHelper.CreateGraphQLUnionTypeExtension(_ignoreOptions);

        extension.Comments = comments;
        ExpectKeyword("union");
        extension.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#UnionTypeExtension");
        extension.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        extension.Types = Peek(TokenKind.EQUALS) ? ParseUnionMemberTypes() : null;
        extension.Location = GetLocation(start);

        if (extension.Directives == null && extension.Types == null)
            return (GraphQLUnionTypeExtension)Throw_Unexpected_Token("; for more information see http://spec.graphql.org/October2021/#UnionTypeExtension");

        DecreaseDepth();
        return extension;
    }

    public GraphQLValue ParseValueLiteral(bool isConstant)
    {
        return _currentToken.Kind switch
        {
            TokenKind.BRACKET_L => ParseListValue(isConstant),
            TokenKind.BRACE_L => ParseObjectValue(isConstant),
            TokenKind.INT => ParseIntValue(),
            TokenKind.FLOAT => ParseFloatValue(),
            TokenKind.STRING => ParseStringValue(),
            TokenKind.NAME => ParseNameValue(),
            TokenKind.DOLLAR when !isConstant => ParseVariable(),
            _ => (GraphQLValue)Throw_Unexpected_Token()
        };
    }

    // http://spec.graphql.org/October2021/#Variable
    public GraphQLVariable ParseVariable()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var variable = NodeHelper.CreateGraphQLVariable(_ignoreOptions);

        variable.Comments = GetComments();
        Expect(TokenKind.DOLLAR);
        variable.Name = ParseName("; for more information see http://spec.graphql.org/October2021/#Variable");
        variable.Location = GetLocation(start);

        DecreaseDepth();
        return variable;
    }

    // http://spec.graphql.org/October2021/#VariableDefinition
    public GraphQLVariableDefinition ParseVariableDefinition()
    {
        IncreaseDepth();

        int start = _currentToken.Start;

        var def = NodeHelper.CreateGraphQLVariableDefinition(_ignoreOptions);

        def.Comments = GetComments();
        def.Variable = ParseVariable();
        Expect(TokenKind.COLON);
        def.Type = ParseType();
        def.DefaultValue = Skip(TokenKind.EQUALS) ? ParseValueLiteral(true) : null;
        def.Directives = Peek(TokenKind.AT) ? ParseDirectives() : null;
        def.Location = GetLocation(start);

        DecreaseDepth();
        return def;
    }
}
