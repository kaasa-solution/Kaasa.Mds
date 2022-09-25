namespace Kaasa.Mds.Services;

public partial class MdsService : IMdsService
{
    private readonly List<MdsDevice> _mdsDevices = new();

    public event EventHandler<string>? OnConnect;

    public event EventHandler<(string macAddr, string serial)>? OnConnectionComplete;

    public event EventHandler<string>? OnDisconnect;

    public event EventHandler<MdsException>? OnError;

    public async Task<IMdsDevice> ConnectAsync(Guid guid)
    {
        var connectTcs = new TaskCompletionSource<IMdsDevice>();

        void onConnectionComplete(object? sender, (string macAddr, string serial) args)
        {
            OnConnectionComplete -= onConnectionComplete;
            OnError -= onError;

            var mdsDevice = new MdsDevice(args.macAddr, args.serial);
            _mdsDevices.Add(mdsDevice);
            connectTcs.SetResult(mdsDevice);
        }

        void onError(object? sender, MdsException exception)
        {
            OnConnectionComplete -= onConnectionComplete;
            OnError -= onError;

            connectTcs.SetException(exception);
        }

        OnConnectionComplete += onConnectionComplete;
        OnError += onError;

        PlatformConnect(guid);

        return await connectTcs.Task.ConfigureAwait(false);
    }

    public async Task DisconnectAsync(IMdsDevice mdsDevice)
    {
        var device = _mdsDevices.FirstOrDefault(x => x.Serial == mdsDevice.Serial);

        if (device == null)
            return;

        var disconnectTcs = new TaskCompletionSource<object?>();

        void onDisconnect(object? sender, string macAddr)
        {
            OnDisconnect -= onDisconnect;
            OnError -= onError;

            _mdsDevices.Remove(device);
            disconnectTcs.SetResult(null);
        }

        void onError(object? sender, MdsException exception)
        {
            OnDisconnect -= onDisconnect;
            OnError -= onError;

            disconnectTcs.SetException(exception);
        }

        OnDisconnect += onDisconnect;
        OnError += onError;

        PlatformDisconnect(device);

        await disconnectTcs.Task.ConfigureAwait(false);
    }
}