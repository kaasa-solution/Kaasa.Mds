using Foundation;
﻿using Kaasa.Mds.iOS;

namespace Kaasa.Mds.Services;

public partial class MdsService : MDSConnectivityServiceDelegate
{
    public MdsService()
    {
        Mds.Current.DoSubscribe("MDS/ConnectedDevices", new NSDictionary(), _ => { }, (@event) => {
            if (@event.BodyDictionary == null)
                return;

            var method = (NSString)@event.BodyDictionary.ValueForKey(new NSString("Method"));

            if (method == new NSString("POST")) {
                var body = (NSDictionary)@event.BodyDictionary.ValueForKey(new NSString("Body"));
                var deviceInfo = (NSDictionary)body.ValueForKey(new NSString("DeviceInfo"));
                var addressInfo = (NSArray)deviceInfo.ValueForKey(new NSString("addressInfo"));
                var macAddr = addressInfo.GetItem<NSDictionary>(0).ValueForKey(new NSString("address")).ToString();
                var connection = (NSDictionary)body.ValueForKey(new NSString("Connection"));
                var uuid = new Guid(((NSString)connection.ValueForKey(new NSString("UUID"))).ToString());
                var serial = ((NSString)body.ValueForKey(new NSString("Serial"))).ToString();

                OnConnect?.Invoke(this, macAddr);
                OnConnectionComplete?.Invoke(this, (uuid, serial));
            } else if (method == new NSString("DEL")) {
                var body = (NSDictionary)@event.BodyDictionary.ValueForKey(new NSString("Body"));
                var serial = ((NSString)body.ValueForKey(new NSString("Serial"))).ToString();

                var device = MdsDevices.FirstOrDefault(x => x.Serial == serial);

                if (device != null)
                    OnDisconnect?.Invoke(this, (device.UUID, device.Serial));
            }
        });
    }

    public override void DidFailToConnectWithError(NSError? error)
    {
        OnError?.Invoke(this, new MdsException(error!.LocalizedDescription));
    }
}