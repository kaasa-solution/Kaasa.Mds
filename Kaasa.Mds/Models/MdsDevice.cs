namespace Kaasa.Mds.Models;

internal sealed class MdsDevice : IMdsDevice
{
    private readonly MdsService _mdsService;

    private readonly ILogger<MdsService> _logger;

    internal List<MdsSubscriptionCall> MdsSubscriptionCalls { get; } = new();

    public Guid UUID { get; }
    public string Serial { get; }
    public string MacAddr { get; }

    public MdsDevice(ILogger<MdsService> logger, MdsService mdsService, Guid uuid, string serial, string macAddr)
    {
        _logger = logger;
        _mdsService = mdsService;
        UUID = uuid;
        Serial = serial;
        MacAddr = macAddr;
    }

    public async Task DisconnectAsync()
    {
        _logger.LogDebug("Disconnecting device {0}.", UUID);
        await new MdsConnectionCall(_logger, _mdsService).DisconnectAsync(this).ConfigureAwait(false);
    }

    public async Task<string?> GetAsync(string path, string prefix = "") 
    { 
        Guard.IsNotNullOrWhiteSpace(path, nameof(path));
        Guard.IsNotNull(prefix, nameof(prefix));

        _logger.LogDebug("Trying to get data at path {0} from device {1}.", path, UUID);

        return await new MdsApiCall(Serial, path).GetAsync(prefix).ConfigureAwait(false);
    }

    public async Task<string?> PutAsync(string path, string contract)
    {
        Guard.IsNotNullOrWhiteSpace(path, nameof(path));
        Guard.IsNotNullOrWhiteSpace(contract, nameof(contract));

        _logger.LogDebug("Trying to create data at path {0} on device {1}.", path, UUID);

        return await new MdsApiCall(Serial, path).PutAsync(contract).ConfigureAwait(false);
    }

    public async Task<string?> PostAsync(string path, string? contract = null)
    {
        Guard.IsNotNullOrWhiteSpace(path, nameof(path));

        _logger.LogDebug("Trying to update data at path {0} on device {1}.", path, UUID);

        return await new MdsApiCall(Serial, path).PostAsync(contract).ConfigureAwait(false);
    }

    public async Task<string?> DeleteAsync(string path)
    {
        Guard.IsNotNullOrWhiteSpace(path, nameof(path));

        _logger.LogDebug("Trying to delete data at path {0} on device {1}.", path, UUID);

        return await new MdsApiCall(Serial, path).DeleteAsync().ConfigureAwait(false);
    }

    public async Task<IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback, bool resubscribe = true)
    {
        Guard.IsNotNullOrWhiteSpace(path, nameof(path));
        Guard.IsNotNull(notificationCallback, nameof(notificationCallback));
        Guard.IsNotNull(resubscribe, nameof(resubscribe));
        
        var subscriptionCall = new MdsSubscriptionCall(this, path, notificationCallback);

        if(resubscribe)
            MdsSubscriptionCalls.Add(subscriptionCall);

        _logger.LogDebug("Add new subscription for data {0} for device {1}.", path, UUID);

        return await subscriptionCall.SubscribeAsync().ConfigureAwait(false);
    }
}