using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.BooleanValue"/>.
/// </summary>
[DebuggerDisplay("GraphQLBooleanValue: {Value}")]
public abstract class GraphQLBooleanValue : GraphQLValue, IHasValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.BooleanValue;

    /// <summary>
    /// Boolean value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value => BoolValue ? "true" : "false";

    /// <summary>
    /// Boolean value represented as <see cref="bool"/>.
    /// </summary>
    public abstract bool BoolValue { get; }
}

/// <summary>
/// AST node for true <see cref="ASTNodeKind.BooleanValue"/>.
/// </summary>
public class GraphQLTrueBooleanValue : GraphQLBooleanValue
{
    /// <inheritdoc/>
    public override bool BoolValue => true;
}

/// <summary>
/// AST node for false <see cref="ASTNodeKind.BooleanValue"/>.
/// </summary>
public class GraphQLFalseBooleanValue : GraphQLBooleanValue
{
    /// <inheritdoc/>
    public override bool BoolValue => false;
}

internal sealed class GraphQLTrueBooleanValueWithLocation : GraphQLTrueBooleanValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLTrueBooleanValueWithComment : GraphQLTrueBooleanValue
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLTrueBooleanValueFull : GraphQLTrueBooleanValue
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

internal sealed class GraphQLFalseBooleanValueWithLocation : GraphQLFalseBooleanValue
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLFalseBooleanValueWithComment : GraphQLFalseBooleanValue
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLFalseBooleanValueFull : GraphQLFalseBooleanValue
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
