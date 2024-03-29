﻿using Kaasa.Mds.Android;

namespace Kaasa.Mds.Services;

internal sealed partial class MdsService : Java.Lang.Object, IMdsConnectionListener
{
    public MdsService(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _serviceLogger = loggerFactory.CreateLogger<MdsService>();
        _deviceLogger = _loggerFactory.CreateLogger<MdsDevice>();
        OnConnectionComplete += MdsServiceOnConnectionComplete;
    }

    void IMdsConnectionListener.OnConnect(string? macAddr)
    {
        OnConnect?.Invoke(this, macAddr!);
    }

    void IMdsConnectionListener.OnConnectionComplete(string? macAddr, string? serial)
    {
        OnConnectionComplete?.Invoke(this, (new Guid($"00000000-0000-0000-0000-{macAddr!.Replace(":", "")}"), serial!));
    }

    void IMdsConnectionListener.OnDisconnect(string? macAddr)
    {
        var device = MdsDevices.FirstOrDefault(x => x.MacAddr == macAddr);

        if (device != null)
            OnDisconnect?.Invoke(this, (device.UUID, device.Serial));
    }

    void IMdsConnectionListener.OnError(Android.MdsException? error)
    {
        OnError?.Invoke(this, new Exceptions.MdsException(error!.Message!));
    }
}
