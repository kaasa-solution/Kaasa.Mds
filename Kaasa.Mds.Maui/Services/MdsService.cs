namespace Kaasa.Mds.Services;

internal sealed partial class MdsService : IMdsService
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
        ArgumentNullException.ThrowIfNull(uuid, nameof(uuid));

        var device = MdsDevices.FirstOrDefault(x => x.UUID == uuid);

        if (device != null)
            return device;

        _serviceLogger.LogTrace("Trying to connect device {uuid}.", uuid);

        return await new MdsConnectionCall(_deviceLogger, this).ConnectAsync(uuid).ConfigureAwait(false);
    }

    public IMdsDevice? GetConnectedSensor(Guid uuid)
    {
        ArgumentNullException.ThrowIfNull(uuid, nameof(uuid));

        return MdsDevices.FirstOrDefault(x => x.UUID == uuid);
    }

    public IMdsDevice? GetConnectedSensor(string serial)
    {
        ArgumentException.ThrowIfNullOrEmpty(serial, nameof(serial));

        return MdsDevices.FirstOrDefault(x => x.Serial == serial);
    }

    private void MdsServiceOnConnectionComplete(object? sender, (Guid uuid, string serial) e)
    {
        ArgumentNullException.ThrowIfNull(e.uuid, nameof(e.uuid));
        ArgumentException.ThrowIfNullOrEmpty(e.serial, nameof(e.serial));

        var device = MdsDevices.FirstOrDefault(x => x.Serial == e.serial);

        if (device is null)
            return;

        _serviceLogger.LogTrace("Trying to establish subscriptions for device {e.uuid}.", e.uuid);

        device.MdsSubscriptionCalls.ForEach(async x => await x.SubscribeAsync().ConfigureAwait(false));
    }
}
