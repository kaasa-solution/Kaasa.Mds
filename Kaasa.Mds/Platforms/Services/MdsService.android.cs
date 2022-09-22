using System.Text.RegularExpressions;

namespace Kaasa.Mds.Services;

public partial class MdsService : Java.Lang.Object, Android.IMdsConnectionListener
{
    private readonly Android.Mds _mds;

    public MdsService()
    {
        if (Mds.Activity == null)
            throw new NotInitializedException();

        _mds = new Android.Mds.Builder().Build(Mds.Activity)!;
    }

    private void PlatformConnect(Guid guid)
    {
        _mds.Connect(Regex.Replace(guid.ToString().Split("-").Last().ToUpper(), ".{2}", "$0:").Remove(17), this);
    }

    private void PlatformDisconnect(IMdsDevice mdsDevice)
    {
        _mds.Disconnect(mdsDevice.MacAddr);
    }

    void Android.IMdsConnectionListener.OnConnect(string? macAddr)
    {
        OnConnect?.Invoke(this, macAddr!);
    }

    void Android.IMdsConnectionListener.OnConnectionComplete(string? macAddr, string? serial)
    {
        var mdsDevice = new MdsDevice(_mds, macAddr!, serial!);
        _mdsDevices.Add(mdsDevice);
        OnConnectionComplete?.Invoke(this, mdsDevice);
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