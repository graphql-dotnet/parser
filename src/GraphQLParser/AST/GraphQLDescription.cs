using System.Diagnostics;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.Description"/>.
/// </summary>
[DebuggerDisplay("GraphQLDescription: {Value}")]
public class GraphQLDescription : ASTNode, IHasValueNode
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.Description;

    /// <summary>
    /// Creates a new instance with the specified value.
    /// </summary>
    public GraphQLDescription(ROM value)
    {
        Value = value;
    }

    /// <summary>
    /// Description value represented as <see cref="ROM"/>.
    /// </summary>
    public ROM Value { get; }
}

internal sealed class GraphQLDescriptionWithLocation : GraphQLDescription
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }

    /// <inheritdoc cref="GraphQLDescription(ROM)"/>
    public GraphQLDescriptionWithLocation(ROM value)
        : base(value)
    {
    }
}
