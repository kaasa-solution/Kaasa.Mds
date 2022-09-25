using Foundation;
using Kaasa.Mds.iOS;
using Kaasa.Mds.Services;

namespace Kaasa.Mds.Models;

internal partial class MdsApiCall : IMdsSubscription
{
    private readonly MDSWrapper _mds;
    private Action<string>? _notificationCallback;

    public MdsApiCall(string serial, string path)
    {
        _mds = MdsService.Mds;
        _serial = serial;
        _path = path;
    }

    public async Task<string?> GetAsync()
    {
        _tcs = new TaskCompletionSource<object?>();

        _mds.DoGet(SchemePrefix + _serial + _path, new NSDictionary(), (x) => {
            if (x.StatusCode == 200) {
                _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8));
            } else {
                _tcs.SetException(new MdsException(x.Description));
            }
        });

        return await _tcs.Task.ConfigureAwait(false) as string;
    }

    public async Task<string?> PutAsync(string contract)
    {
        _tcs = new TaskCompletionSource<object?>();

        var dictionary = (NSDictionary)NSJsonSerialization.Deserialize(NSData.FromString(contract), NSJsonReadingOptions.MutableContainers, out NSError error);

        if (dictionary != null) {
            _mds.DoPut(SchemePrefix + _serial + _path, dictionary, (x) => {
                if (x.StatusCode == 200) {
                    _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8));
                } else {
                    _tcs.SetException(new MdsException(x.Description));
                }
            });
        } else {
            _tcs.SetException(new MdsException(error.Description));
        }

        return await _tcs.Task.ConfigureAwait(false) as string;
    }

    public async Task<string?> PostAsync(string? contract = null)
    {
        _tcs = new TaskCompletionSource<object?>();

        var dictionary = (NSDictionary)NSJsonSerialization.Deserialize(NSData.FromString(contract), NSJsonReadingOptions.MutableContainers, out NSError error);

        if (dictionary != null) {
            _mds.DoPost(SchemePrefix + _serial + _path, dictionary, (x) => {
                if (x.StatusCode == 200) {
                    _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8));
                } else {
                    _tcs.SetException(new MdsException(x.Description));
                }
            });
        } else {
            _tcs.SetException(new MdsException(error.Description));
        }

        return await _tcs.Task.ConfigureAwait(false) as string;
    }

    public async Task<string?> DeleteAsync()
    {
        _tcs = new TaskCompletionSource<object?>();

        _mds.DoDelete(SchemePrefix + _serial + _path, new NSDictionary(), (x) => {
            if (x.StatusCode == 200) {
                _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8));
            } else {
                _tcs.SetException(new MdsException(x.Description));
            }
        });

        return await _tcs.Task.ConfigureAwait(false) as string;
    }

    public async Task<IMdsSubscription> SubscribeAsync(Action<string> notificationCallback)
    {
        _tcs = new TaskCompletionSource<object?>();

        _notificationCallback = notificationCallback;

        _mds.DoSubscribe(_path, new NSDictionary(), (x) => {
            if (x.StatusCode == 200) {
                _tcs?.SetResult(this);
            } else {
                _tcs?.SetException(new MdsException(x.Description));
            }
        }, (x) => {
            _notificationCallback?.Invoke(new NSString(x.BodyData, NSStringEncoding.UTF8));
        });

        return (await _tcs.Task.ConfigureAwait(false) as IMdsSubscription)!;
    }

    public void Unsubscribe()
    {
        _mds.DoUnsubscribe(_path);
    }
}