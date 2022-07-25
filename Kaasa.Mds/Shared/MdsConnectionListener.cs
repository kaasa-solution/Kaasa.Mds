namespace Kaasa.Mds;

public partial class MdsConnectionListener
{
    /// <summary>
    /// Event fires when a device connects.
    /// Contains the PhysicalAddress (MAC).
    /// </summary>
    public event EventHandler<string>? SensorConnected;

    /// <summary>
    /// Event fires when connection has completed.
    /// Contains the UUID and Serial.
    /// </summary>
    public event EventHandler<(Guid uuid, string serial)>? SensorConnectionComplete;

    /// <summary>
    /// Event fires when a device disconnects.
    /// Contains the UUID and Serial.
    /// </summary>
    public event EventHandler<(Guid uuid, string serial)>? SensorDisconnected;

    /// <summary>
    /// Event fires when MdsLib reports unexpected connection error.
    /// </summary>
    public event EventHandler<Exception>? SensorConnectionError;
}