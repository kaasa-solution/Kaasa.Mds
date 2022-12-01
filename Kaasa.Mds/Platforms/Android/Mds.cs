using Android.App;

﻿namespace Kaasa.Mds;

public static partial class Mds
{
    private static readonly Lazy<Android.Mds> _current = new(() => new Android.Mds.Builder().Build(Activity)!, true);
    internal static Android.Mds Current => _current.Value;

    private static Activity? _activity;

    private static Activity? Activity {
        get {
            if (_activity == null)
                throw new NotInitializedException();

            return _activity;
        }
        set {
            _activity = value;
        }
    }

    public static void Init(Activity activity)
    {
        Activity = activity;
    }
}