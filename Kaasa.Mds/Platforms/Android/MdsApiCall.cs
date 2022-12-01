using Kaasa.Mds.Android;

namespace Kaasa.Mds.Models;

internal sealed partial class MdsApiCall : Java.Lang.Object, IMdsResponseListener
{
    public async Task<string?> GetAsync(string prefix = "")
    {
        Mds.Current.Get(SchemePrefix + prefix + _serial + _path, null, this);

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string?> PutAsync(string contract)
    {
        Mds.Current.Put(SchemePrefix + _serial + _path, contract, this);

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string?> PostAsync(string? contract = null)
    {
        Mds.Current.Post(SchemePrefix + _serial + _path, contract, this);

        return await _tcs.Task.ConfigureAwait(false);
    }

    public async Task<string?> DeleteAsync()
    {
        Mds.Current.Delete(SchemePrefix + _serial + _path, null, this);

        return await _tcs.Task.ConfigureAwait(false);
    }

    public void OnSuccess(string? data, MdsHeader? mdsHeader)
    {
        _tcs.SetResult(data);
    }

    public void OnError(Android.MdsException? error)
    {
        _tcs.SetException(new Exceptions.MdsException(error!.Message!));
    }
}