using Kaasa.Mds.Services;

namespace Kaasa.Mds.Models;

internal partial class MdsConnectionCall
{
    public MdsConnectionCall(MdsService mdsService)
    {
        throw new NotImplementedException();
    }

    public async Task<IMdsDevice> ConnectAsync(Guid uuid)
    {
        throw new NotImplementedException();
    }

    public async Task DisconnectAsync(MdsDevice mdsDevice)
    {
        throw new NotImplementedException();
    }
}