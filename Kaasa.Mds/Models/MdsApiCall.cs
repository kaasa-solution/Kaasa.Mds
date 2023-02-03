namespace Kaasa.Mds.Models;

internal sealed partial class MdsApiCall
{
    private const string SchemePrefix = "suunto://";

    private readonly string _serial;
    private readonly string _path;
    private readonly TaskCompletionSource<string> _tcs = new();

    public MdsApiCall(string serial, string path)
    {
        _serial = serial;
        _path = path.StartsWith("/") ? path : "/" + path;
    }
}