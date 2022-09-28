namespace Kaasa.Mds.Models;

internal sealed partial class MdsConnectionCall
{
    private readonly TaskCompletionSource<object?> _tcs = new();
    private readonly MdsService _mdsService;

    public MdsConnectionCall(MdsService mdsService)
    {
        _mdsService = mdsService;
    }
}