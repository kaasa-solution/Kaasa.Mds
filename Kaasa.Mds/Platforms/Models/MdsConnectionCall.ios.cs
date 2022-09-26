using Foundation;
using Kaasa.Mds.Services;

namespace Kaasa.Mds.Models;

internal partial class MdsConnectionCall
{
    public MdsConnectionCall(MdsService mdsService)
    {
        _mdsService = mdsService;
    }

    public async Task<IMdsDevice> ConnectAsync(Guid uuid)
    {
        _tcs = new TaskCompletionSource<object?>();

        string? macAddr = null;

        void onConnect(object? sender, string? _macAddr)
        {
            macAddr = _macAddr!;
        }

        void onConnectionComplete(object? sender, (Guid uuid, string serial) e)
        {
            _mdsService.OnConnect -= onConnect;
            _mdsService.OnConnectionComplete -= onConnectionComplete;
            _mdsService.OnError -= onError;

            var device = new MdsDevice(e.uuid, e.serial, macAddr ?? string.Empty);

            _mdsService.MdsDevices.Add(device);
            _tcs.SetResult(device);
        }

        void onError(object? sender, MdsException exception)
        {
            _mdsService.OnConnect -= onConnect;
            _mdsService.OnConnectionComplete -= onConnectionComplete;
            _mdsService.OnError -= onError;

            _tcs.SetException(exception);
        }

        _mdsService.OnConnect += onConnect;
        _mdsService.OnConnectionComplete += onConnectionComplete;
        _mdsService.OnError += onError;

        var nsuuid = new NSUuid(uuid.ToString());

        // Fix for the ios connection bug
        var source = new CancellationTokenSource();
        _ = Task.Run(() => Mds.Current.ConnectPeripheralWithUUID(nsuuid), source.Token);
        await Task.Delay(TimeSpan.FromSeconds(0.2));
        source.Cancel();
        source.Dispose();
        await Task.Delay(TimeSpan.FromSeconds(0.2));
        Mds.Current.ConnectPeripheralWithUUID(nsuuid);

        return (await _tcs.Task.ConfigureAwait(false) as IMdsDevice)!;
    }

    public async Task DisconnectAsync(MdsDevice mdsDevice)
    {
        var _tcs = new TaskCompletionSource<object?>();

        void onDisconnect(object? sender, (Guid uuid, string serial) e)
        {
            _mdsService.OnDisconnect -= onDisconnect;
            _mdsService.OnError -= onError;

            _mdsService.MdsDevices.Remove(mdsDevice);
            _tcs.SetResult(null);
        }

        void onError(object? sender, MdsException exception)
        {
            _mdsService.OnDisconnect -= onDisconnect;
            _mdsService.OnError -= onError;

            _tcs.SetException(exception);
        }

        _mdsService.OnDisconnect += onDisconnect;
        _mdsService.OnError += onError;

        Mds.Current.DisconnectPeripheralWithUUID(new NSUuid(mdsDevice.UUID.ToString()));

        await _tcs.Task.ConfigureAwait(false);
    }
}