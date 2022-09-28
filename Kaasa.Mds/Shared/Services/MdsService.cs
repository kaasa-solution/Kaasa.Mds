namespace Kaasa.Mds.Services;

public sealed partial class MdsService : IMdsService
{
    internal List<MdsDevice> MdsDevices { get; } = new();

    public event EventHandler<string>? OnConnect;

    public event EventHandler<(Guid uuid, string serial)>? OnConnectionComplete;

    public event EventHandler<(Guid uuid, string serial)>? OnDisconnect;

    public event EventHandler<MdsException>? OnError;

    public async Task<IMdsDevice> ConnectAsync(Guid uuid)
    {
        var device = MdsDevices.FirstOrDefault(x => x.UUID == uuid);

        if (device != null)
            return device;

        return await new MdsConnectionCall(this).ConnectAsync(uuid).ConfigureAwait(false);
    }

    public IMdsDevice? GetConnectedSensor(Guid uuid)
    {
        return MdsDevices.FirstOrDefault(x => x.UUID == uuid);
    }

    public IMdsDevice? GetConnectedSensor(string serial)
    {
        return MdsDevices.FirstOrDefault(x => x.Serial == serial);
    }
}