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
    event EventHandler<(string macAddr, string serial)>? OnConnectionComplete;

    /// <summary>
    /// Called when Mds connection disconnects (e.g. device out of range).
    /// </summary>
    event EventHandler<string>? OnDisconnect;

    /// <summary>
    /// Called when Mds connect() call fails with error.
    /// </summary>
    event EventHandler<MdsException>? OnError;

    /// <summary>
    /// Connect to a device.
    /// </summary>
    /// <param name="guid">Guid of the device to be connected</param>
    /// <returns>Returns the connected <see cref="IMdsDevice"/> on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<IMdsDevice> ConnectAsync(Guid guid);

    /// <summary>
    /// Disconnect a device.
    /// </summary>
    /// <param name="mdsDevice">Device to disconnect</param>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task DisconnectAsync(IMdsDevice mdsDevice);
}