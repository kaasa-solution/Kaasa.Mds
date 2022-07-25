using Kaasa.Mds.Android;

namespace Kaasa.Mds;

public partial class MdsConnectionListener : Java.Lang.Object, IMdsConnectionListener
{
    private Dictionary<Guid, string> _connectedSensors = new();

    public void OnConnect(string? physicalAddress)
    {
        if (string.IsNullOrWhiteSpace(physicalAddress))
            return;

        SensorConnected?.Invoke(this, physicalAddress!);
    }

    public void OnConnectionComplete(string? physicalAddress, string? serial)
    {
        if (string.IsNullOrWhiteSpace(physicalAddress) || string.IsNullOrWhiteSpace(serial))
            return;

        var uuid = GetUUIDFromPhysicalAddress(physicalAddress!);

        if (uuid == null || uuid == Guid.Empty)
            return;

        SensorConnectionComplete?.Invoke(this, (uuid, serial!));
    }

    public void OnDisconnect(string? physicalAddress)
    {
        if (string.IsNullOrWhiteSpace(physicalAddress))
            return;

        var uuid = GetUUIDFromPhysicalAddress(physicalAddress!);

        if (uuid == null || uuid == Guid.Empty)
            return;

        if (!_connectedSensors.TryGetValue(uuid, out var serial))
            return;

        SensorDisconnected?.Invoke(this, (uuid, serial));
        _connectedSensors.Remove(uuid);
    }

    public void OnError(MdsException? exception)
    {
        if (exception?.Message?.StartsWith("com.polidea.rxandroidble.exceptions.BleDisconnectedException") ?? false) {
            var msgParts = exception.Message.Split(" ");

            if (msgParts == null ||msgParts.Length == 0)
                return;

            var uuid = GetUUIDFromPhysicalAddress(msgParts[msgParts.Length - 1]);

            if (uuid == null || uuid == Guid.Empty)
                return;

            if (!_connectedSensors.TryGetValue(uuid, out var serial))
                return;

            SensorDisconnected?.Invoke(this, (uuid, serial));
        } else {
            SensorConnectionError?.Invoke(this, new(exception?.Message ?? "Unexpected exception"));
        }
    }

    private Guid GetUUIDFromPhysicalAddress(string physicalAddress)
    {
        return new Guid($"00000000-0000-0000-0000-{physicalAddress.Replace(":", "")}");
    }
}