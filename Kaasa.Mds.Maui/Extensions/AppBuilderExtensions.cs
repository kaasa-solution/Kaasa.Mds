namespace Kaasa.Mds.Maui.Extensions;

/// <summary>
/// Provides extension methods for configuring a Maui application with the Kaasa.Mds library.
/// </summary>
public static class AppBuilderExtensions
{
    /// <summary>
    /// Registers the <see cref="IMdsService"/> with the application's service collection.
    /// </summary>
    /// <param name="builder">The <see cref="MauiAppBuilder"/> instance.</param>
    /// <returns>The updated <see cref="MauiAppBuilder"/> instance.</returns>
    public static MauiAppBuilder UseKaasaMds(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IMdsService, MdsService>();
        return builder;
    }
}
