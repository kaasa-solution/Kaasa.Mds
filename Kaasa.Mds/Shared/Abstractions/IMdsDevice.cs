﻿namespace Kaasa.Mds.Abstractions;

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
    /// Get a resource.
    /// </summary>
    /// <param name="path">Resources path</param>
    /// <returns>Returns a json string (can be null or empty) on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<string?> GetAsync(string path);

    /// <summary>
    /// Create a resource.
    /// </summary>
    /// <param name="path">Resources path</param>
    /// <param name="contract">Outgoing data as json</param>
    /// <returns>Returns a json string (can be null or empty) on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<string?> PutAsync(string path, string contract);

    /// <summary>
    /// Update a resource.
    /// </summary>
    /// <param name="path">Resources path</param>
    /// <param name="contract">Outgoing data as json (if necessary)</param>
    /// <returns>Returns a json string (can be null or empty) on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<string?> PostAsync(string path, string? contract = null);

    /// <summary>
    /// Delete a resource.
    /// </summary>
    /// <param name="path">Resources path</param>
    /// <returns>Returns a json string (can be null or empty) on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<string?> DeleteAsync(string path);

    /// <summary>
    /// Subscribe notifications.
    /// </summary>
    /// <param name="path">Resource path</param>
    /// <param name="notificationCallback">Callback for the data</param>
    /// <returns>Returns a <see cref="IMdsSubscription"/> on success</returns>
    /// <exception cref="MdsException">Thrown when an error occurs</exception>
    Task<IMdsSubscription> SubscribeAsync(string path, Action<string> notificationCallback);
}