namespace Kaasa.Mds.Abstractions;

public interface IMdsDevice
{
    string MacAddr { get; }

    string Serial { get; }

    Task<string?> GetAsync(string path);

    Task<string?> PutAsync(string path, string contract);

    Task<string?> PostAsync(string path, string? contract = null);

    Task<string?> DeleteAsync(string path);

    Task<IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback);
}