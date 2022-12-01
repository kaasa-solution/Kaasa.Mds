using System.Text.RegularExpressions;

namespace Kaasa.Mds.Models;

internal sealed partial class MdsConnectionCall : Java.Lang.Object
{
    public async Task<IMdsDevice> ConnectAsync(Guid uuid)
    {
        var macAddr = Regex.Replace(uuid.ToString().Split("-").Last().ToUpper(), ".{2}", "$0:").Remove(17);

        void onConnectionComplete(object? sender, (Guid uuid, string serial) e)
        {
            _mdsService.OnConnectionComplete -= onConnectionComplete;
            _mdsService.OnError -= onError;

            var device = new MdsDevice(_mdsService, e.uuid, e.serial, macAddr);

            _mdsService.MdsDevices.Add(device);
            _tcs.SetResult(device);
        }

        void onError(object? sender, MdsException exception)
        {
            _mdsService.OnConnectionComplete -= onConnectionComplete;
            _mdsService.OnError -= onError;

            _tcs.SetException(exception);
        }

        _mdsService.OnConnectionComplete += onConnectionComplete;
        _mdsService.OnError += onError;

        Mds.Current.Connect(macAddr, _mdsService);

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

        Mds.Current.Disconnect(mdsDevice.MacAddr);

        await _tcs.Task.ConfigureAwait(false);
    }
}