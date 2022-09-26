using Kaasa.Mds.Services;

namespace Kaasa.Mds.Models;

internal partial class MdsConnectionCall
{
    private readonly MdsService _mdsService;
    private TaskCompletionSource<object?>? _tcs;
}