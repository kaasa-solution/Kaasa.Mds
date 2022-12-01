﻿using Kaasa.Mds.Android;

namespace Kaasa.Mds.Models;

internal sealed partial class MdsSubscriptionCall : Java.Lang.Object, IMdsNotificationListener
{
    private Android.IMdsSubscription? _mdsSubscription;

    public async Task<Abstractions.IMdsSubscription> SubscribeAsync()
    {
        _tcs = new();

        var subscribtionPath = Path[0] == '/' ? Path.Remove(0, 1) : Path;
        _mdsSubscription = Mds.Current.Subscribe(SchemePrefix, "{\"Uri\": \"" + MdsDevice.Serial + "/" + subscribtionPath + "\"}", this);

        return await _tcs.Task.ConfigureAwait(false);
    }

    public void Unsubscribe()
    {
        _mdsSubscription?.Unsubscribe();
        _mdsSubscription = null;
        ((MdsDevice)MdsDevice).MdsSubscriptionCalls.Remove(this);
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