using Kaasa.Mds.Models;

namespace Kaasa.Mds.Services;

public partial class MdsService : IMdsService
{
    private readonly List<MdsDevice> _mdsDevices = new();

    private TaskCompletionSource<IMdsDevice>? _connectTcs;
    private TaskCompletionSource<object?>? _disconnectTcs;

    public event EventHandler<string>? OnConnect;

    public event EventHandler<IMdsDevice>? OnConnectionComplete;

    public event EventHandler<string>? OnDisconnect;

    public event EventHandler<MdsException>? OnError;

    public async Task<IMdsDevice> ConnectAsync(Guid guid)
    {
        _connectTcs = new();

        void onConnectionComplete(object? sender, IMdsDevice mdsDevice)
        {
            OnConnectionComplete -= onConnectionComplete;
            OnError -= onError;

            _connectTcs.SetResult(mdsDevice);
        }

        void onError(object? sender, MdsException exception)
        {
            OnConnectionComplete -= onConnectionComplete;
            OnError -= onError;

            _connectTcs.SetException(exception);
        }

        OnConnectionComplete += onConnectionComplete;
        OnError += onError;

        PlatformConnect(guid);

        return await _connectTcs.Task.ConfigureAwait(false);
    }

    public async Task DisconnectAsync(IMdsDevice mdsDevice)
    {
        var device = _mdsDevices.FirstOrDefault(x => x.Serial == mdsDevice.Serial);

        if (device == null)
            return;

        _disconnectTcs = new();

        void onDisconnect(object? sender, string macAddr)
        {
            OnDisconnect -= onDisconnect;
            OnError -= onError;

            _mdsDevices.Remove(device);
            _disconnectTcs.SetResult(null);
        }

        void onError(object? sender, MdsException exception)
        {
            OnDisconnect -= onDisconnect;
            OnError -= onError;

            _disconnectTcs.SetException(exception);
        }

        OnDisconnect += onDisconnect;
        OnError += onError;

        PlatformDisconnect(device);

        await _disconnectTcs.Task.ConfigureAwait(false);
    }
}