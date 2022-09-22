#if !NET6_0_OR_GREATER
using Foundation;
#endif

using Kaasa.Mds.iOS;

namespace Kaasa.Mds.Services;

public partial class MdsService : MDSConnectivityServiceDelegate
{
    private readonly MDSWrapper _mds = new();

    public MdsService()
    {
        _mds.DoSubscribe("MDS/ConnectedDevices", new NSDictionary(), _ => { }, (@event) => {
            if (@event.BodyDictionary == null)
                return;

            var method = (NSString)@event.BodyDictionary.ValueForKey(new NSString("Method"));

            if (method == new NSString("POST")) {
                var body = (NSDictionary)@event.BodyDictionary.ValueForKey(new NSString("Body"));
                var serial = ((NSString)body.ValueForKey(new NSString("Serial"))).ToString();
                var connection = (NSDictionary)body.ValueForKey(new NSString("Connection"));
                var uuid = ((NSString)connection.ValueForKey(new NSString("UUID"))).ToString();
                // TODO CHECK FOR MACADRESS
                var mdsDevice = new MdsDevice(_mds, uuid, serial);
                _mdsDevices.Add(mdsDevice);

                OnConnect?.Invoke(this, uuid);
                OnConnectionComplete?.Invoke(this, mdsDevice);
            } else if (method == new NSString("DEL")) {
                var body = (NSDictionary)@event.BodyDictionary.ValueForKey(new NSString("Body"));
                var serial = ((NSString)body.ValueForKey(new NSString("Serial"))).ToString();

                OnDisconnect?.Invoke(this, serial);
            }
        });
    }

    private void PlatformConnect(Guid guid)
    {
        var uuid = new NSUuid(guid.ToString());

        if(uuid != null)
            _mds.ConnectPeripheralWithUUID(uuid);
    }

    private void PlatformDisconnect(IMdsDevice mdsDevice)
    {
        var uuid = new NSUuid(mdsDevice.MacAddr);

        if (uuid != null)
            _mds.DisconnectPeripheralWithUUID(uuid);
    }

    public override void DidFailToConnectWithError(NSError? error)
    {
        OnError?.Invoke(this, new MdsException(error!.LocalizedDescription));
    }
}