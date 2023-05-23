namespace Kaasa.Mds;

internal static partial class Mds
{
    private static readonly Lazy<Android.Mds> _current = new(() => new Android.Mds.Builder().Build(Platform.CurrentActivity)!, true);
    internal static Android.Mds Current => _current.Value;
}
