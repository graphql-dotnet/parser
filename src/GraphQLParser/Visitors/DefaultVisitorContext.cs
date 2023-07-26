namespace GraphQLParser.Visitors;

/// <summary>
/// An implementation of <see cref="IASTVisitorContext"/> that only contains a <see cref="CancellationToken"/>.
/// Ideal for use in cases where there is no context variables.
/// </summary>
public struct DefaultVisitorContext : IASTVisitorContext
{
    /// <inheritdoc/>
    public CancellationToken CancellationToken { get; set; }
}
