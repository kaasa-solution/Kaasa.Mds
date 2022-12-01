namespace Kaasa.Mds.Models;

internal sealed class MdsDevice : IMdsDevice
{
    private readonly MdsService _mdsService;

    internal List<MdsSubscriptionCall> MdsSubscriptionCalls { get; } = new();

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
        await new MdsConnectionCall(_mdsService).DisconnectAsync(this).ConfigureAwait(false);

    public async Task<string?> GetAsync(string path, string prefix = "") =>
        await new MdsApiCall(Serial, path).GetAsync(prefix).ConfigureAwait(false);

    public async Task<string?> PutAsync(string path, string contract) =>
        await new MdsApiCall(Serial, path).PutAsync(contract).ConfigureAwait(false);

    public async Task<string?> PostAsync(string path, string? contract = null) =>
        await new MdsApiCall(Serial, path).PostAsync(contract).ConfigureAwait(false);

    public async Task<string?> DeleteAsync(string path) =>
        await new MdsApiCall(Serial, path).DeleteAsync().ConfigureAwait(false);

    public async Task<IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback, bool resubscribe = true)
    {
        var subscriptionCall = new MdsSubscriptionCall(this, path, notificationCallback);

        if(resubscribe)
            MdsSubscriptionCalls.Add(subscriptionCall);

        return await subscriptionCall.SubscribeAsync().ConfigureAwait(false);
    }
}