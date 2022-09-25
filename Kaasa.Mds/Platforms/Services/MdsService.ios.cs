using Kaasa.Mds.iOS;

namespace Kaasa.Mds.Services;

public partial class MdsService : MDSConnectivityServiceDelegate
{
    internal static MDSWrapper Mds { get; } = new();

    public MdsService()
    {
        Mds.DoSubscribe("MDS/ConnectedDevices", new NSDictionary(), _ => { }, (@event) => {
            if (@event.BodyDictionary == null)
                return;

            var method = (NSString)@event.BodyDictionary.ValueForKey(new NSString("Method"));

            if (method == new NSString("POST")) {
                var body = (NSDictionary)@event.BodyDictionary.ValueForKey(new NSString("Body"));
                var serial = ((NSString)body.ValueForKey(new NSString("Serial"))).ToString();
                var connection = (NSDictionary)body.ValueForKey(new NSString("Connection"));
                var uuid = ((NSString)connection.ValueForKey(new NSString("UUID"))).ToString();
                // TODO CHECK FOR MACADRESS

                OnConnect?.Invoke(this, uuid);
                OnConnectionComplete?.Invoke(this, (uuid, serial));
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

        if (uuid != null)
            Mds.ConnectPeripheralWithUUID(uuid);
    }

    private void PlatformDisconnect(IMdsDevice mdsDevice)
    {
        var uuid = new NSUuid(mdsDevice.MacAddr);

        if (uuid != null)
            Mds.DisconnectPeripheralWithUUID(uuid);
    }

    public override void DidFailToConnectWithError(NSError? error)
    {
        OnError?.Invoke(this, new MdsException(error!.LocalizedDescription));
    }
}