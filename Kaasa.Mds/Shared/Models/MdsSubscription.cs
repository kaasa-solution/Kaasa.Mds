namespace Kaasa.Mds.Models;

internal partial class MdsSubscription : IMdsSubscription
{
    private const string SchemePrefix = "suunto://MDS/EventListener";

    private readonly string _serial;
    private readonly string _path;
    private TaskCompletionSource<IMdsSubscription>? _tcs;
}