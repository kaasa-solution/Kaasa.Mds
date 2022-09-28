using Foundation;

namespace Kaasa.Mds.Models;

internal sealed partial class MdsSubscriptionCall
{
    public async Task<IMdsSubscription> SubscribeAsync()
    {
        Mds.Current.DoSubscribe(_serial + _path, new NSDictionary(), (x) => {
            if (x.StatusCode == 200) {
                _tcs?.SetResult(this);
            } else {
                _tcs?.SetException(new MdsException(x.Description));
            }
        }, (x) => {
            _notificationCallback?.Invoke(new NSString(x.BodyData, NSStringEncoding.UTF8).ToString());
        });

        return await _tcs.Task.ConfigureAwait(false);
    }

    public void Unsubscribe()
    {
        Mds.Current.DoUnsubscribe(_serial + _path);
    }
}