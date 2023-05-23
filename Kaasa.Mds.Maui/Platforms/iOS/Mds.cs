using Kaasa.Mds.iOS;

namespace Kaasa.Mds;

internal static partial class Mds
{
    private static readonly Lazy<MDSWrapper> _current = new(() => new(), true);
    internal static MDSWrapper Current => _current.Value;
}
