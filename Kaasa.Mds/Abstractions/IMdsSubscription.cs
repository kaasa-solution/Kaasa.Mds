namespace Kaasa.Mds.Abstractions;

/// <summary>
/// Represents a Mds subscription.
/// </summary>
public interface IMdsSubscription
{
    /// <summary>
    /// <see cref="IMdsDevice"/> on which the subscription runs.
    /// </summary>
    IMdsDevice MdsDevice { get; }

    /// <summary>
    /// Resources path.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Unsubscribe notifications.
    /// </summary>
    void Unsubscribe();
}
