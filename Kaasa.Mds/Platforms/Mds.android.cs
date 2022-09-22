#if !NET6_0_OR_GREATER
using Android.App;
#endif

namespace Kaasa.Mds;

public partial class Mds
{
    internal static Activity? Activity { get; private set; }

    public static void Init(Activity activity)
    {
        Activity = activity;
    }
}