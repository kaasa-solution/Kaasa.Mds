namespace Kaasa.Mds.Models;

internal partial class MdsDevice : IMdsDevice
{
    public string MacAddr { get; }
    public string Serial { get; }

    public MdsDevice(string macAddr, string serial)
    {
        MacAddr = macAddr;
        Serial = serial;
    }

    public async Task<string?> GetAsync(string path) =>
        await new MdsApiCall(Serial, path).GetAsync();

    public async Task<string?> PutAsync(string path, string contract) =>
        await new MdsApiCall(Serial, path).PutAsync(contract);

    public async Task<string?> PostAsync(string path, string? contract = null) =>
        await new MdsApiCall(Serial, path).PostAsync(contract);

    public async Task<string?> DeleteAsync(string path) =>
        await new MdsApiCall(Serial, path).DeleteAsync();

    public async Task<IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback) =>
        await new MdsApiCall(Serial, path).SubscribeAsync(notificationCallback);
}