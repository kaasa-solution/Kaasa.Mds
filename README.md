# Kaasa.Mds

The following plugin enables a cross platform usage of the movesense device library for Android/iOS and .NET 7.0 (MAUI).
In case of errors or suggestions for changes we are grateful for every pull request!

The project is composed of 3 parts.

- Kaasa.Mds: This repository that wraps the native Android/iOS projects.
- Kaasa.Mds.Android: The Android Binding project for the native library.
- Kaasa.Mds.iOS: The iOS binding project for the native library.

Kaasa.Mds.Android can be found here: https://github.com/kaasa-solution/Kaasa.Mds.Android

Kaasa.Mds.iOS can be found here: https://github.com/kaasa-solution/Kaasa.Mds.iOS

The movesense project can be found here: https://bitbucket.org/movesense/movesense-device-lib

## Installation

The plugin is available on NuGet.

* NuGet Official Releases: [![NuGet](https://img.shields.io/nuget/v/Kaasa.Mds?label=NuGet)](https://www.nuget.org/packages/Kaasa.Mds)

Browse with the NuGet manager in your IDE to install them or run this command:

`Install-Package Kaasa.Mds`

## Getting Started

The functions of the plugin are documented in detail in the code, but for a better understanding, here is a brief overview.

- The IMdsService is used to connect sensors and provides callbacks for the connection status or possible errors. Internally a list of all connected sensors is kept permanently. After successful connection of a sensor it returns an IMdsDevice (see below). Already connected sensors can be retrieved at any time by their id or serial number.

- The IMdsDevice mirrors a connected sensor and allows to execute Api calls like Get or Subscribe. Also the disconnect of the sensor is done via the IMdsDevice itself. If the reference to an IMdsDevice is lost, it can be retrieved at any time via the IMdsService. Difference to other Mds Libraries lies here in the Subscribe method. Since the IMdsService keeps a list of all connected sensors, subscriptions can be renewed automatically if a sensor loses the connection and then restored. This "resubscribe" parameter is on by default, but can be changed when calling SubscribeAsync.

- The IMdsSubscription is a reference to a successful subscription and is used to remove it. It also contains some information like the path to which the subscription was made or the device that has the subscription. This is used to associate subscriptions and devices.

The plugin is designed to be initialized e.g. via the maui dependency injection. For this the IMdsService is first registered in the MauiProgram class.

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        ...
        builder.Services.AddSingleton<IMdsService, MdsService>();
        ...

        return builder.Build();
    }
}
```

The IMdsService can now be injected into any other class such as a ViewModel. Afterwards sensors can be connected, for a more detailed description of the IMdsService see above.

```csharp
public sealed partial class ViewModel
{
    private readonly IMdsService _mdsService;

    public ViewModel(IMdsService mdsService)
    {
        _mdsService = mdsService;
    }
}
```

To connect a sensor, the device id of the sensor, which was found with the Plugin.Ble plugin for example, must be passed to the IMdsService. 
This returns an IMdsDevice if the connection was successful.

```csharp
public sealed partial class ViewModel
{
    private readonly IMdsService _mdsService;

    public async Task ConnectCommand()
    {
        try {
            var mdsDevice = await _mdsService.ConnectAsync(ID);
            .. 
            await mdsDevice.DisconnectAsync();
        }
    }
}
```

The plugin intentionally omits the implementation of individual movesense apis, but provides a simple and clear structure to include them. One possibility is to extend the existing IMds interfaces with functionality. An example would be a method that extends the IMdsDevice with the GetTime functionality.

```csharp
public static IMdsDeviceExtensions
{
    public static async Task<Time> GetTimeAsync(this IMdsDevice mdsDevice)
    {
        var json = await mdsDevice.GetAsync("/Time");
        return JsonConvert.DeserializeObject<Time>(json);
    }
}
```

Finding Bluetooth devices is not the responsibility of the Mds plugin and must be implemented additionally. 
See e.g. https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le.

The permissions required are the same as for the Ble scan itself (see Plugin.Ble). It is up to the user of the plugin to request these and insert them correctly into e.g. the Android manifest.
