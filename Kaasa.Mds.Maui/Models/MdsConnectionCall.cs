namespace Kaasa.Mds.Models;

internal sealed partial class MdsConnectionCall
{
    private readonly ILogger<MdsDevice> _logger;
    private readonly MdsService _mdsService;
    private readonly TaskCompletionSource<object?> _tcs = new();

    public MdsConnectionCall(ILogger<MdsDevice> logger, MdsService mdsService)
    {
        _logger = logger;
        _mdsService = mdsService;
    }
}
