using System.ComponentModel;
using ElysiaFramework;
using System.Windows.Media;
using ClassIsland.Services;
using ElysiaFramework.Interfaces;
using Microsoft.Extensions.Hosting;

namespace StickyHomeworks.Services;

public class ThemeBackgroundService : IHostedService
{
    private SettingsService SettingsService { get; }

    private IThemeService ThemeService { get; }

    private WallpaperPickingService WallpaperPickingService { get; }

    public ThemeBackgroundService(SettingsService settingsService, IThemeService themeService, WallpaperPickingService wallpaperPickingService)
    {
        SettingsService = settingsService;
        ThemeService = themeService;
        WallpaperPickingService = wallpaperPickingService;
        SettingsService.OnSettingsChanged += SettingsServiceOnOnSettingsChanged;
        
    }

    private void SettingsServiceOnOnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        UpdateTheme();
    }

    private void UpdateTheme()
    {
        var primary = Colors.DodgerBlue;
        var secondary = Colors.DodgerBlue;
        switch (SettingsService.Settings.ColorSource)
        {
            case 0: //custom
                primary = SettingsService.Settings.PrimaryColor;
                secondary = SettingsService.Settings.SecondaryColor;
                break;
            case 1:
                primary = secondary = SettingsService.Settings.SelectedPlatte;
                break;
            case 2:
                try
                {
                    NativeWindowHelper.DwmGetColorizationColor(out var color, out _);
                    var c = NativeWindowHelper.GetColor(color);
                    primary = secondary = c;
                }
                catch
                {
                    // ignored
                }

                break;
        }

        ThemeService.SetTheme(SettingsService.Settings.Theme, primary, secondary);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        UpdateTheme();
        await WallpaperPickingService.GetWallpaperAsync();
        UpdateTheme();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}