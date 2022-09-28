namespace Kaasa.Mds.Models;

internal sealed partial class MdsConnectionCall
{
    public Task<IMdsDevice> ConnectAsync(Guid uuid)
    {
        throw new NotImplementedException();
    }

    public Task DisconnectAsync(MdsDevice mdsDevice)
    {
        throw new NotImplementedException();
    }
}