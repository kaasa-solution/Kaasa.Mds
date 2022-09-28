namespace Kaasa.Mds.Models;

internal sealed partial class MdsApiCall
{
    private const string SchemePrefix = "suunto://";
    private readonly TaskCompletionSource<string?> _tcs = new();
    private readonly string _serial;
    private readonly string _path;

    public MdsApiCall(string serial, string path)
    {
        _serial = serial;
        _path = path;
    }
}