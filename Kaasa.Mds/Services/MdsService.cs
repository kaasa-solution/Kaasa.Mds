using Java.Util;

namespace Kaasa.Mds.Services;

public sealed partial class MdsService : IMdsService
{
    private readonly ILogger<MdsService> _logger;
    internal List<MdsDevice> MdsDevices { get; } = new();

    public event EventHandler<string>? OnConnect;

    public event EventHandler<(Guid uuid, string serial)>? OnConnectionComplete;

    public event EventHandler<(Guid uuid, string serial)>? OnDisconnect;

    public event EventHandler<MdsException>? OnError;

    public async Task<IMdsDevice> ConnectAsync(Guid uuid)
    {
        Guard.IsNotNull(uuid, nameof(uuid));

        var device = MdsDevices.FirstOrDefault(x => x.UUID == uuid);

        _logger.LogDebug("Trying to connect device {0}.", uuid);

        if (device != null)
            return device;

        return await new MdsConnectionCall(this).ConnectAsync(uuid).ConfigureAwait(false);
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

        _logger.LogDebug("Trying to establish subscriptions for device {0}.", e.uuid);

        var device = MdsDevices.FirstOrDefault(x => x.Serial == e.serial);

        if (device is null)
            return;

        device.MdsSubscriptionCalls.ForEach(async x => await x.SubscribeAsync().ConfigureAwait(false));
    }
}