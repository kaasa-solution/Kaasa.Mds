using Foundation;

namespace Kaasa.Mds.Models;

internal sealed partial class MdsConnectionCall
{
    public async Task<IMdsDevice> ConnectAsync(Guid uuid)
    {
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

            var device = new MdsDevice(_mdsService, e.uuid, e.serial, macAddr ?? string.Empty);

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

        Mds.Current.ConnectPeripheralWithUUID(new NSUuid(uuid.ToString()));

        return (await _tcs.Task.ConfigureAwait(false) as IMdsDevice)!;
    }

    public async Task DisconnectAsync(MdsDevice mdsDevice)
    {
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