﻿namespace Kaasa.Mds.Services;

public sealed partial class MdsService : IMdsService
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<MdsService> _serviceLogger;
    private readonly ILogger<MdsDevice> _deviceLogger;
    internal List<MdsDevice> MdsDevices { get; } = new();

    public event EventHandler<string>? OnConnect;

    public event EventHandler<(Guid uuid, string serial)>? OnConnectionComplete;

    public event EventHandler<(Guid uuid, string serial)>? OnDisconnect;

    public event EventHandler<MdsException>? OnError;

    public async Task<IMdsDevice> ConnectAsync(Guid uuid)
    {
        Guard.IsNotNull(uuid, nameof(uuid));

        var device = MdsDevices.FirstOrDefault(x => x.UUID == uuid);

        if (device != null)
            return device;

        _serviceLogger.LogTrace("Trying to connect device {uuid}.", uuid);

        return await new MdsConnectionCall(_deviceLogger, this).ConnectAsync(uuid).ConfigureAwait(false);
    }

    public IMdsDevice? GetConnectedSensor(Guid uuid)
    {
        Guard.IsNotNull(uuid, nameof(uuid));

        return MdsDevices.FirstOrDefault(x => x.UUID == uuid);
    }

    public IMdsDevice? GetConnectedSensor(string serial)
    {
        Guard.IsNotNullOrWhiteSpace(serial, nameof(serial));

        return MdsDevices.FirstOrDefault(x => x.Serial == serial);
    }

    private void MdsServiceOnConnectionComplete(object? sender, (Guid uuid, string serial) e)
    {
        Guard.IsNotNull(e.uuid, nameof(e.uuid));
        Guard.IsNotNullOrWhiteSpace(e.serial, nameof(e.serial));

        var device = MdsDevices.FirstOrDefault(x => x.Serial == e.serial);

        if (device is null)
            return;

        _serviceLogger.LogTrace("Trying to establish subscriptions for device {e.uuid}.", e.uuid);

        device.MdsSubscriptionCalls.ForEach(async x => await x.SubscribeAsync().ConfigureAwait(false));
    }
}
