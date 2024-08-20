using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputValueDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLInputValueDefinition: {Name}: {Type}")]
public class GraphQLInputValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode, IHasDefaultValueNode
{
    internal GraphQLInputValueDefinition()
    {
        Type = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInputValueDefinition"/>.
    /// </summary>
    [Obsolete($"Please use the {nameof(GraphQLArgumentDefinition)} or {nameof(GraphQLInputFieldDefinition)} constructor.")]
    public GraphQLInputValueDefinition(GraphQLName name, GraphQLType type)
        : base(name)
    {
        Type = type;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.InputValueDefinition;

    /// <summary>
    /// Nested <see cref="GraphQLType"/> AST node with input value type.
    /// </summary>
    public GraphQLType Type { get; set; }

    /// <inheritdoc />
    public GraphQLValue? DefaultValue { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc />
    public override bool IsChildDefinition => true;
}

internal sealed class GraphQLInputValueDefinitionWithLocation : GraphQLInputValueDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLInputValueDefinitionWithComment : GraphQLInputValueDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLInputValueDefinitionFull : GraphQLInputValueDefinition
{
    private GraphQLLocation _location;
    private List<GraphQLComment>? _comments;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputValueDefinition"/>, where it is used as an argument definition.
/// </summary>
[DebuggerDisplay("GraphQLArgumentDefinition: {Name}: {Type}")]
public class GraphQLArgumentDefinition : GraphQLInputValueDefinition
{
    internal GraphQLArgumentDefinition() : base() { }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLArgumentDefinition"/>.
    /// </summary>
    public GraphQLArgumentDefinition(GraphQLName name, GraphQLType type)
#pragma warning disable CS0618 // Type or member is obsolete
        : base(name, type) { }
#pragma warning restore CS0618 // Type or member is obsolete
}

internal sealed class GraphQLArgumentDefinitionWithLocation : GraphQLArgumentDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLArgumentDefinitionWithComment : GraphQLArgumentDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLArgumentDefinitionFull : GraphQLArgumentDefinition
{
    private GraphQLLocation _location;
    private List<GraphQLComment>? _comments;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

/// <summary>
/// AST node for <see cref="ASTNodeKind.InputValueDefinition"/>, where it is used as an input field definition.
/// </summary>
[DebuggerDisplay("GraphQLInputFieldDefinition: {Name}: {Type}")]
public class GraphQLInputFieldDefinition : GraphQLInputValueDefinition
{
    internal GraphQLInputFieldDefinition() : base() { }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLInputFieldDefinition"/>.
    /// </summary>
    public GraphQLInputFieldDefinition(GraphQLName name, GraphQLType type)
#pragma warning disable CS0618 // Type or member is obsolete
        : base(name, type) { }
#pragma warning restore CS0618 // Type or member is obsolete
}

internal sealed class GraphQLInputFieldDefinitionWithLocation : GraphQLInputFieldDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLInputFieldDefinitionWithComment : GraphQLInputFieldDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLInputFieldDefinitionFull : GraphQLInputFieldDefinition
{
    private GraphQLLocation _location;
    private List<GraphQLComment>? _comments;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}
