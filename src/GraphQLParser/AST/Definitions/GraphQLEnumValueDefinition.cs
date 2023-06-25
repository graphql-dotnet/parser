using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.EnumValueDefinition"/>.
/// </summary>
[DebuggerDisplay("GraphQLEnumValueDefinition: {EnumValue}")]
public class GraphQLEnumValueDefinition : GraphQLTypeDefinition, IHasDirectivesNode
{
    /// <summary>Initializes a new instance.</summary>
    [Obsolete("This constructor will be removed in v9.")]
    public GraphQLEnumValueDefinition()
    {
        EnumValue = null!;
    }

    /// <summary>
    /// Creates a new instance of <see cref="GraphQLEnumValueDefinition"/>.
    /// </summary>
    public GraphQLEnumValueDefinition(GraphQLName name, GraphQLEnumValue enumValue)
        : base(name)
    {
        EnumValue = enumValue;
    }

    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.EnumValueDefinition;

    /// <summary>
    /// Enum value represented as a nested AST node. Alas, inherited
    /// <see cref="GraphQLTypeDefinition.Name"/> property holds almost
    /// the same data and should be set as well.
    /// </summary>
    public GraphQLEnumValue EnumValue { get; set; }

    /// <inheritdoc/>
    public GraphQLDirectives? Directives { get; set; }

    /// <inheritdoc />
    public override bool IsChildDefinition => true;
}

internal sealed class GraphQLEnumValueDefinitionWithLocation : GraphQLEnumValueDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLEnumValueDefinitionWithComment : GraphQLEnumValueDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLEnumValueDefinitionFull : GraphQLEnumValueDefinition
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
