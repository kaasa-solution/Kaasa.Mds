using Kaasa.Mds.iOS;

namespace Kaasa.Mds.Models;

internal partial class MdsDevice
{
    private readonly MDSWrapper _mds;

    public MdsDevice(MDSWrapper mds, string macAddr, string serial)
    {
        _mds = mds;
        MacAddr = macAddr;
        Serial = serial;
    }

    public async Task<string?> GetAsync(string path) =>
        await new MdsApiCall(_mds, Serial, path).GetAsync();

    public async Task<string?> PutAsync(string path, string contract) =>
        await new MdsApiCall(_mds, Serial, path).PutAsync(contract);

    public async Task<string?> PostAsync(string path, string? contract = null) =>
        await new MdsApiCall(_mds, Serial, path).PostAsync(contract);

    public async Task<string?> DeleteAsync(string path) =>
        await new MdsApiCall(_mds, Serial, path).DeleteAsync();

    public async Task<Abstractions.IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback) =>
        await new MdsSubscription(_mds, Serial, path, notificationCallback).SubscribeAsync();
}