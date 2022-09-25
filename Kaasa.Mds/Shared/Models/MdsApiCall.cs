namespace Kaasa.Mds.Models;

internal partial class MdsApiCall
{
    private const string SchemePrefix = "suunto://";
    private readonly string _serial;
    private readonly string _path;
    private TaskCompletionSource<object?>? _tcs;
}