namespace Kaasa.Mds.Exceptions;

public sealed class MdsException : Exception
{
    internal MdsException(string message) : base(message)
    {
    }
}
