namespace Kaasa.Mds.Models;

internal partial class MdsDevice : IMdsDevice
{
    public string MacAddr { get; }
    public string Serial { get; }
}