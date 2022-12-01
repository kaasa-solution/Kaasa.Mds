namespace Kaasa.Mds.Models;

internal sealed partial class MdsConnectionCall
{
    private readonly TaskCompletionSource<object?> _tcs = new();
    private readonly MdsService _mdsService;
    private readonly ILogger<MdsService> _logger;

    public MdsConnectionCall(ILogger<MdsService> logger, MdsService mdsService)
    {
        _logger = logger;
        _mdsService = mdsService;
    }
}