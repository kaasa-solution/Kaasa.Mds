using Foundation;

namespace Kaasa.Mds.Models;

internal sealed partial class MdsSubscriptionCall
{
    public async Task<IMdsSubscription> SubscribeAsync()
    {
        _tcs = new();

        Mds.Current.DoSubscribe(MdsDevice.Serial + Path, new NSDictionary(), (x) => {
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
        Mds.Current.DoUnsubscribe(MdsDevice.Serial + Path);
        ((MdsDevice)MdsDevice).MdsSubscriptionCalls.Remove(this);
    }
}
