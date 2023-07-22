namespace GraphQLParser.Visitors;

/// <summary>
/// An implementation of <see cref="IASTVisitorContext"/> that does nothing.
/// Ideal for use in cases where the visitor runs synchronously, there is no context
/// variables, and cancellation is not required.
/// </summary>
public struct NullVisitorContext : IASTVisitorContext
{
    /// <inheritdoc/>
    public readonly CancellationToken CancellationToken => default;
}
