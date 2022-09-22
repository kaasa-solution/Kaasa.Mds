using Kaasa.Mds.Android;

namespace Kaasa.Mds.Models;

internal partial class MdsSubscription : Java.Lang.Object, IMdsNotificationListener
{
    private readonly Android.Mds _mds;
    private Android.IMdsSubscription? _mdsSubscription;
    private Action<string> _notificationCallback;

    public MdsSubscription(Android.Mds mds, string serial, string path, Action<string> notificationCallback)
    {
        _mds = mds;
        _serial = serial;

        if (path.Substring(0, 1) == "/") {
            _path = path.Remove(0, 1);
        } else {
            _path = path;
        }

        _notificationCallback = notificationCallback;        
    }

    public async Task<Abstractions.IMdsSubscription> SubscribeAsync()
    {
        _tcs = new TaskCompletionSource<Abstractions.IMdsSubscription>();

        _mdsSubscription = _mds.Subscribe(SchemePrefix, "{\"Uri\": \"" + _serial + "/" + _path + "\"}", this);

        return await _tcs.Task.ConfigureAwait(false);
    }

    public void Unsubscribe()
    {
        _mdsSubscription?.Unsubscribe();
    }

    public void OnNotification(string? data)
    {
        if(!_tcs?.Task.IsCompleted ?? false)
            _tcs?.SetResult(this);

        _notificationCallback?.Invoke(data!);
    }

    public void OnError(Android.MdsException? error)
    {
        _tcs?.SetException(new Exceptions.MdsException(error!.Message ?? string.Empty));
    }
}