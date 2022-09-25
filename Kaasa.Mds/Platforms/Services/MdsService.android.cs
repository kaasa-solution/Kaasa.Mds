using System.Text.RegularExpressions;

namespace Kaasa.Mds.Services;

public partial class MdsService : Java.Lang.Object, Android.IMdsConnectionListener
{
    internal static Android.Mds? Mds { get; private set; }

    public MdsService()
    {
        if (Kaasa.Mds.Mds.Activity == null)
            throw new NotInitializedException();

        Mds ??= new Android.Mds.Builder().Build(Kaasa.Mds.Mds.Activity)!;
    }

    private void PlatformConnect(Guid guid)
    {
        Mds!.Connect(Regex.Replace(guid.ToString().Split("-").Last().ToUpper(), ".{2}", "$0:").Remove(17), this);
    }

    private void PlatformDisconnect(IMdsDevice mdsDevice)
    {
        Mds!.Disconnect(mdsDevice.MacAddr);
    }

    void Android.IMdsConnectionListener.OnConnect(string? macAddr)
    {
        OnConnect?.Invoke(this, macAddr!);
    }

    void Android.IMdsConnectionListener.OnConnectionComplete(string? macAddr, string? serial)
    {
        OnConnectionComplete?.Invoke(this, (macAddr!, serial!));
    }

    void Android.IMdsConnectionListener.OnDisconnect(string? macAddr)
    {
        OnDisconnect?.Invoke(this, macAddr!);
    }

    void Android.IMdsConnectionListener.OnError(Android.MdsException? error)
    {
        OnError?.Invoke(this, new MdsException(error!.Message!));
    }
}