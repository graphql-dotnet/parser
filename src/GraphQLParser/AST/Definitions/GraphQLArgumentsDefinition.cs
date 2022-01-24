using System.Collections.Generic;

namespace GraphQLParser.AST;

/// <summary>
/// AST node for <see cref="ASTNodeKind.ArgumentsDefinition"/>.
/// </summary>
public class GraphQLArgumentsDefinition : ASTListNode<GraphQLInputValueDefinition>
{
    /// <inheritdoc/>
    public override ASTNodeKind Kind => ASTNodeKind.ArgumentsDefinition;
}

internal sealed class GraphQLArgumentsDefinitionWithLocation : GraphQLArgumentsDefinition
{
    private GraphQLLocation _location;

    public override GraphQLLocation Location
    {
        get => _location;
        set => _location = value;
    }
}

internal sealed class GraphQLArgumentsDefinitionWithComment : GraphQLArgumentsDefinition
{
    private List<GraphQLComment>? _comments;

    public override List<GraphQLComment>? Comments
    {
        get => _comments;
        set => _comments = value;
    }
}

internal sealed class GraphQLArgumentsDefinitionFull : GraphQLArgumentsDefinition
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
