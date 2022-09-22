using Kaasa.Mds.Android;

namespace Kaasa.Mds.Models;

internal partial class MdsApiCall : Java.Lang.Object, IMdsResponseListener
{
	private readonly Android.Mds _mds;

	public MdsApiCall(Android.Mds mds, string serial, string path)
	{
		_mds = mds;
		_serial = serial;	
		_path = path;
	}

	public async Task<string?> GetAsync()
	{
		_tcs = new TaskCompletionSource<string?>();

		_mds.Get(SchemePrefix + _serial + _path, null, this);

		return await _tcs.Task.ConfigureAwait(false);
	}

	public async Task<string?> PutAsync(string contract)
	{
		_tcs = new TaskCompletionSource<string?>();

		_mds.Put(SchemePrefix + _serial + _path, contract, this);

		return await _tcs.Task.ConfigureAwait(false);
	}

	public async Task<string?> PostAsync(string? contract = null)
	{
		_tcs = new TaskCompletionSource<string?>();

		_mds.Post(SchemePrefix + _serial + _path, contract, this);

		return await _tcs.Task.ConfigureAwait(false);
	}

	public async Task<string?> DeleteAsync()
	{
		_tcs = new TaskCompletionSource<string?>();

		_mds.Get(SchemePrefix + _serial + _path, null, this);

		return await _tcs.Task.ConfigureAwait(false);
	}

    public void OnSuccess(string? data, MdsHeader? mdsHeader)
	{
		_tcs?.SetResult(data);
	}

    public void OnError(Android.MdsException? error)
	{
		_tcs?.SetException(new Exceptions.MdsException(error!.Message ?? string.Empty));
	}
}