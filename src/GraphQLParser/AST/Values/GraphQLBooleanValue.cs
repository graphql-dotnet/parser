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
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLTrueBooleanValueWithComment : GraphQLTrueBooleanValue
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLTrueBooleanValueFull : GraphQLTrueBooleanValue
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLFalseBooleanValueWithLocation : GraphQLFalseBooleanValue
{
    public override GraphQLLocation Location { get; set; }
}

internal sealed class GraphQLFalseBooleanValueWithComment : GraphQLFalseBooleanValue
{
    public override List<GraphQLComment>? Comments { get; set; }
}

internal sealed class GraphQLFalseBooleanValueFull : GraphQLFalseBooleanValue
{
    public override GraphQLLocation Location { get; set; }

    public override List<GraphQLComment>? Comments { get; set; }
}
