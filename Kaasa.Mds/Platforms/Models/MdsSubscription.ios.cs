using Kaasa.Mds.iOS;

namespace Kaasa.Mds.Models;

internal partial class MdsSubscription
{
    private readonly MDSWrapper _mds;
    private Action<string> _notificationCallback;

    public MdsSubscription(MDSWrapper mds, string serial, string path, Action<string> notificationCallback)
    {
        _mds = mds;
        _serial = serial;
        _path = path;
        _notificationCallback = notificationCallback;
    }

    public async Task<IMdsSubscription> SubscribeAsync()
    {
        _tcs = new TaskCompletionSource<IMdsSubscription>();

        _mds.DoSubscribe(_path, new NSDictionary(), (x) => {
            if (x.StatusCode == 200) {
                _tcs?.SetResult(this);
            } else {
                _tcs?.SetException(new MdsException(x.Description));
            }
        }, (x) => {
            _notificationCallback?.Invoke(new NSString(x.BodyData, NSStringEncoding.UTF8));
        });

        return await _tcs.Task.ConfigureAwait(false);
    }

    public void Unsubscribe()
    {
        _mds.DoUnsubscribe(_path);
    }
}