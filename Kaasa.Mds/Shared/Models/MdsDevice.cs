namespace Kaasa.Mds.Models;

internal sealed class MdsDevice : IMdsDevice
{
    private readonly MdsService _mdsService;

    public Guid UUID { get; }
    public string Serial { get; }
    public string MacAddr { get; }

    public MdsDevice(MdsService mdsService, Guid uuid, string serial, string macAddr)
    {
        _mdsService = mdsService;
        UUID = uuid;
        Serial = serial;
        MacAddr = macAddr;
    }

    public async Task DisconnectAsync() =>
        await new MdsConnectionCall(_mdsService).DisconnectAsync(this);

    public async Task<string?> GetAsync(string path) =>
        await new MdsApiCall(Serial, path).GetAsync().ConfigureAwait(false);

    public async Task<string?> PutAsync(string path, string contract) =>
        await new MdsApiCall(Serial, path).PutAsync(contract).ConfigureAwait(false);

    public async Task<string?> PostAsync(string path, string? contract = null) =>
        await new MdsApiCall(Serial, path).PostAsync(contract).ConfigureAwait(false);

    public async Task<string?> DeleteAsync(string path) =>
        await new MdsApiCall(Serial, path).DeleteAsync().ConfigureAwait(false);

    public async Task<IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback) =>
        await new MdsSubscriptionCall(Serial, path, notificationCallback).SubscribeAsync().ConfigureAwait(false);
}