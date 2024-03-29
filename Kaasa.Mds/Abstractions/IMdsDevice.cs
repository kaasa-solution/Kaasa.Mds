﻿namespace Kaasa.Mds.Abstractions;

/// <summary>
/// Represents a Mds device.
/// </summary>
public interface IMdsDevice
{
    /// <summary>
    /// UUID of the device.
    /// </summary>
    Guid UUID { get; }

    /// <summary>
    /// Serial of the device.
    /// </summary>
    string Serial { get; }

    /// <summary>
    /// MAC-Address of the device.
    /// </summary>
    string MacAddr { get; }

    /// <summary>
    /// Disconnect the device.
    /// </summary>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task DisconnectAsync();

    /// <summary>
    /// Get a resource.
    /// </summary>
    /// <param name="path">Resources path</param>
    /// <param name="prefix">Prefix to be inserted in the path before the serial number</param>
    /// <param name="contract">Outgoing data as json (if necessary)</param>
    /// <returns>Returns a json string on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<string> GetAsync(string path, string prefix = "", string? contract = null);

    /// <summary>
    /// Create a resource.
    /// </summary>
    /// <param name="path">Resources path</param>
    /// <param name="contract">Outgoing data as json</param>
    /// <returns>Returns a json string on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<string> PutAsync(string path, string contract);

    /// <summary>
    /// Update a resource.
    /// </summary>
    /// <param name="path">Resources path</param>
    /// <param name="contract">Outgoing data as json (if necessary)</param>
    /// <returns>Returns a json string on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<string> PostAsync(string path, string? contract = null);

    /// <summary>
    /// Delete a resource.
    /// </summary>
    /// <param name="path">Resources path</param>
    /// <returns>Returns a json string on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<string> DeleteAsync(string path);

    /// <summary>
    /// Subscribe notifications.
    /// </summary>
    /// <param name="path">Resource path</param>
    /// <param name="notificationCallback">Callback for the data</param>
    /// <param name="resubscribe">Determines if the subscription should be automatically renewed when the sensor reconnects</param>
    /// <returns>Returns a <see cref="IMdsSubscription"/> on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback, bool resubscribe = true);
}
