namespace Kaasa.Mds.Exceptions;

public sealed class MdsException : Exception
{
    public MdsException(string message) : base(message)
    {
    }
}