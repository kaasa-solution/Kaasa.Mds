namespace Kaasa.Mds.Models;

internal sealed partial class MdsApiCall
{
    public Task<string?> GetAsync(string prefix = "")
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
}