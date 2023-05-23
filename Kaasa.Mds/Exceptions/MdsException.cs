namespace Kaasa.Mds.Exceptions;

/// <summary>
/// Represents a Mds exception.
/// </summary>
public sealed class MdsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MdsException"/> class.
    /// </summary>
    /// <param name="message"></param>
    public MdsException(string message) : base(message)
    {
    }
}
