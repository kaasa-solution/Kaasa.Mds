namespace Kaasa.Mds.Models;

internal class MdsDevice : IMdsDevice
{
    public Guid UUID { get; }
    public string Serial { get; }
    public string MacAddr { get; }

    public MdsDevice(Guid uuid, string serial, string macAddr)
    {
        UUID = uuid;
        Serial = serial;
        MacAddr = macAddr;
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