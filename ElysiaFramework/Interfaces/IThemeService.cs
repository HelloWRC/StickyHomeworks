using Microsoft.Extensions.Hosting;
using System;
using System.Windows.Media;

namespace ElysiaFramework.Interfaces;

public interface IThemeService : IHostedService
{
    public event EventHandler<ThemeUpdatedEventArgs>? ThemeUpdated;

    public int CurrentRealThemeMode { get; set; }

    public void SetTheme(int themeMode, Color primary, Color secondary);
}