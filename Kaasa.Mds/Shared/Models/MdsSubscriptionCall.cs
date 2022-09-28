namespace Kaasa.Mds.Models;

internal sealed partial class MdsSubscriptionCall : IMdsSubscription
{
    private const string SchemePrefix = "suunto://MDS/EventListener";
    private readonly TaskCompletionSource<IMdsSubscription> _tcs = new();
    private readonly string _serial;
    private readonly string _path;
    private Action<string>? _notificationCallback;

    public MdsSubscriptionCall(string serial, string path, Action<string> notificationCallback)
    {
        _serial = serial;
        _path = path;
        _notificationCallback = notificationCallback;
    }
}