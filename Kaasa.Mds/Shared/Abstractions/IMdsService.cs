namespace Kaasa.Mds.Abstractions;

public interface IMdsService
{
    /// <summary>
    /// Called when Mds / Whiteboard link-layer connection (BLE) has been succesfully established.
    /// </summary>
    event EventHandler<string>? OnConnect;

    /// <summary>
    /// Called when the full Mds / Whiteboard connection has been succesfully established.
    /// </summary>
    event EventHandler<IMdsDevice>? OnConnectionComplete;

    /// <summary>
    /// Called when Mds connection disconnects (e.g. device out of range)
    /// </summary>
    event EventHandler<string>? OnDisconnect;

    /// <summary>
    /// Called when Mds connect() call fails with error.
    /// </summary>
    event EventHandler<MdsException>? OnError;

    Task<IMdsDevice> ConnectAsync(Guid guid);

    Task DisconnectAsync(IMdsDevice mdsDevice);
}