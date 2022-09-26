﻿using Kaasa.Mds.Android;

namespace Kaasa.Mds.Models;

internal partial class MdsApiCall : Java.Lang.Object, IMdsResponseListener, IMdsNotificationListener, Abstractions.IMdsSubscription
{
    private Android.IMdsSubscription? _mdsSubscription;

    public MdsApiCall(string serial, string path)
    {
        _serial = serial;
        _path = path;
    }

    public async Task<string?> GetAsync()
    {
        _tcs = new TaskCompletionSource<object?>();

        Mds.Current.Get(SchemePrefix + _serial + _path, null, this);

        return await _tcs.Task.ConfigureAwait(false) as string;
    }

    public async Task<string?> PutAsync(string contract)
    {
        _tcs = new TaskCompletionSource<object?>();

        Mds.Current.Put(SchemePrefix + _serial + _path, contract, this);

        return await _tcs.Task.ConfigureAwait(false) as string;
    }

    public async Task<string?> PostAsync(string? contract = null)
    {
        _tcs = new TaskCompletionSource<object?>();

        Mds.Current.Post(SchemePrefix + _serial + _path, contract, this);

        return await _tcs.Task.ConfigureAwait(false) as string;
    }

    public async Task<string?> DeleteAsync()
    {
        _tcs = new TaskCompletionSource<object?>();

        Mds.Current.Delete(SchemePrefix + _serial + _path, null, this);

        return await _tcs.Task.ConfigureAwait(false) as string;
    }

    public async Task<Abstractions.IMdsSubscription> SubscribeAsync(Action<string> notificationCallback)
    {
        _tcs = new TaskCompletionSource<object?>();

        _notificationCallback = notificationCallback;

        string subscribtionPath;

        if (_path.Substring(0, 1) == "/") {
            subscribtionPath = _path.Remove(0, 1);
        } else {
            subscribtionPath = _path;
        }

        _mdsSubscription = Mds.Current.Subscribe("suunto://MDS/EventListener", "{\"Uri\": \"" + _serial + "/" + subscribtionPath + "\"}", this);

        return (await _tcs.Task.ConfigureAwait(false) as Abstractions.IMdsSubscription)!;
    }

    public void Unsubscribe()
    {
        _mdsSubscription?.Unsubscribe();
    }

    public void OnSuccess(string? data, MdsHeader? mdsHeader)
    {
        _tcs?.SetResult(data);
    }

    public void OnNotification(string? data)
    {
        if (!_tcs?.Task.IsCompleted ?? false)
            _tcs?.SetResult(this);

        _notificationCallback?.Invoke(data!);
    }

    public void OnError(Android.MdsException? error)
    {
        _tcs?.SetException(new Exceptions.MdsException(error!.Message ?? string.Empty));
    }
}