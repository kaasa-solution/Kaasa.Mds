using Kaasa.Mds.iOS;

namespace Kaasa.Mds.Models;

internal partial class MdsApiCall
{
    private readonly MDSWrapper _mds;

    public MdsApiCall(MDSWrapper mds, string serial, string path)
    {
        _mds = mds;
        _serial = serial;
        _path = path;
    }

    public async Task<string?> GetAsync()
    {
        _tcs = new TaskCompletionSource<string?>();

        _mds.DoGet(SchemePrefix + _serial + _path, new NSDictionary(), (x) => {
            if (x.StatusCode == 200) {
                _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8));
            } else {
                _tcs.SetException(new MdsException(x.Description));
            }
        });

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string?> PutAsync(string contract)
    {
        _tcs = new TaskCompletionSource<string?>();

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

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string?> PostAsync(string? contract = null)
    {
        _tcs = new TaskCompletionSource<string?>();

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

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string?> DeleteAsync()
    {
        _tcs = new TaskCompletionSource<string?>();

        _mds.DoDelete(SchemePrefix + _serial + _path, new NSDictionary(), (x) => {
            if (x.StatusCode == 200) {
                _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8));
            } else {
                _tcs.SetException(new MdsException(x.Description));
            }
        });

        return await _tcs.Task.ConfigureAwait(false);
    }
}