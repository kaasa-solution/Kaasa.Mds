namespace Kaasa.Mds.Models;

internal partial class MdsApiCall
{
    public MdsApiCall(string serial, string path)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string?> PutAsync(string contract)
    {
        throw new NotImplementedException();
    }

    public Task<string?> PostAsync(string? contract = null)
    {
        throw new NotImplementedException();
    }

    public Task<string?> DeleteAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IMdsSubscription> SubscribeAsync(Action<string> notificationCallback)
    {
        throw new NotImplementedException();
    }
}