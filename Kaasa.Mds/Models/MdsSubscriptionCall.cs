namespace Kaasa.Mds.Models;

internal sealed partial class MdsSubscriptionCall : IMdsSubscription
{
    private const string SchemePrefix = "suunto://MDS/EventListener";

    private readonly Action<string>? _notificationCallback;
    private TaskCompletionSource<IMdsSubscription>? _tcs;

    public IMdsDevice MdsDevice { get; }
    public string Path { get; }

    public MdsSubscriptionCall(IMdsDevice mdsDevice, string path, Action<string> notificationCallback)
    {
        MdsDevice = mdsDevice;
        Path = path.StartsWith("/") ? path : "/" + path; ;
        _notificationCallback = notificationCallback;
    }
}