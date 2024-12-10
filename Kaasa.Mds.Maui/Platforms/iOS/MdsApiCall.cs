using Foundation;

namespace Kaasa.Mds.Models;

internal sealed partial class MdsApiCall
{
    public async Task<string> GetAsync(string prefix = "", string? contract = null)
    {
        NSDictionary? dictionary;
        NSError? error = null;

        if (!string.IsNullOrWhiteSpace(contract)) {
            dictionary = (NSDictionary) NSJsonSerialization.Deserialize(NSData.FromString(contract!), NSJsonReadingOptions.MutableContainers, out error);
        } else {
            dictionary = new();
        }

        if (dictionary != null) {
            Mds.Current.DoGet(SchemePrefix + prefix + _serial + _path, dictionary, (x) => {
                if (x.StatusCode == 200) {
                    _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8).ToString());
                } else {
                    _tcs.SetException(new MdsException(x.Description));
                }
            });
        } else {
            _tcs.SetException(new MdsException(error!.Description));
        }

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string> PutAsync(string contract)
    {
        NSDictionary dictionary = new();
        NSError returnedError = new();

        if(contract.Length != 0)
        {
            dictionary = (NSDictionary) NSJsonSerialization.Deserialize(NSData.FromString(contract), NSJsonReadingOptions.MutableContainers, out NSError error);
            returnedError = error;
        }

        if (dictionary != null) {
            Mds.Current.DoPut(SchemePrefix + _serial + _path, dictionary, (x) => {
                if (x.StatusCode == 200) {
                    _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8).ToString());
                } else {
                    _tcs.SetException(new MdsException(x.Description));
                }
            });
        } else {
            _tcs.SetException(new MdsException(returnedError.Description));
        }

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string> PostAsync(string? contract = null)
    {
        NSDictionary? dictionary;
        NSError? error = null;

        if (!string.IsNullOrWhiteSpace(contract)) {
            dictionary = (NSDictionary) NSJsonSerialization.Deserialize(NSData.FromString(contract!), NSJsonReadingOptions.MutableContainers, out error);
        } else {
            dictionary = new();
        }

        if (dictionary != null) {
            Mds.Current.DoPost(SchemePrefix + _serial + _path, dictionary, (x) => {
                if (x.StatusCode == 200 || x.StatusCode == 201) {
                    _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8).ToString());
                } else {
                    _tcs.SetException(new MdsException(x.Description));
                }
            });
        } else {
            _tcs.SetException(new MdsException(error!.Description));
        }

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string> DeleteAsync()
    {
        Mds.Current.DoDelete(SchemePrefix + _serial + _path, new NSDictionary(), (x) => {
            if (x.StatusCode == 200) {
                _tcs.SetResult(new NSString(x.BodyData, NSStringEncoding.UTF8).ToString());
            } else {
                _tcs.SetException(new MdsException(x.Description));
            }
        });

        return await _tcs.Task.ConfigureAwait(false);
    }
}
