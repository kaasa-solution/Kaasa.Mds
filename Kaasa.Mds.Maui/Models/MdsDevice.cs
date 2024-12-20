﻿namespace Kaasa.Mds.Models;

internal sealed class MdsDevice : IMdsDevice
{
    private readonly ILogger<MdsDevice> _logger;
    private readonly MdsService _mdsService;

    internal List<MdsSubscriptionCall> MdsSubscriptionCalls { get; } = new();

    public Guid UUID { get; }
    public string Serial { get; }
    public string MacAddr { get; }

    public MdsDevice(ILogger<MdsDevice> logger, MdsService mdsService, Guid uuid, string serial, string macAddr)
    {
        _logger = logger;
        _mdsService = mdsService;
        UUID = uuid;
        Serial = serial;
        MacAddr = macAddr;
    }

    public async Task DisconnectAsync()
    {
        _logger.LogTrace("Disconnecting device {UUID}.", UUID);
        await new MdsConnectionCall(_logger, _mdsService).DisconnectAsync(this).ConfigureAwait(false);
    }

    public async Task<string> GetAsync(string path, string prefix = "", string? contract = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        ArgumentNullException.ThrowIfNull(prefix, nameof(prefix));

        _logger.LogTrace("Trying to get data at path {path} from device {UUID}.", path, UUID);

        return await new MdsApiCall(Serial, path).GetAsync(prefix, contract).ConfigureAwait(false);
    }

    public async Task<string> PutAsync(string path, string contract)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        _logger.LogTrace("Trying to create data at path {path} on device {UUID}.", path, UUID);

        return await new MdsApiCall(Serial, path).PutAsync(contract).ConfigureAwait(false);
    }

    public async Task<string> PostAsync(string path, string? contract = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        _logger.LogTrace("Trying to update data at path {path} on device {UUID}.", path, UUID);

        return await new MdsApiCall(Serial, path).PostAsync(contract).ConfigureAwait(false);
    }

    public async Task<string> DeleteAsync(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        _logger.LogTrace("Trying to delete data at path {path} on device {UUID}.", path, UUID);

        return await new MdsApiCall(Serial, path).DeleteAsync().ConfigureAwait(false);
    }

    public async Task<IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback, bool resubscribe = true)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        ArgumentNullException.ThrowIfNull(notificationCallback, nameof(notificationCallback));
        ArgumentNullException.ThrowIfNull(resubscribe, nameof(resubscribe));

        var subscriptionCall = new MdsSubscriptionCall(this, path, notificationCallback);

        if (resubscribe)
            MdsSubscriptionCalls.Add(subscriptionCall);

        _logger.LogTrace("Add new subscription for data {path} for device {UUID}.", path, UUID);

        return await subscriptionCall.SubscribeAsync().ConfigureAwait(false);
    }
}
