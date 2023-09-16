using System.Configuration;
using System.Data;
using System.Windows;
using ElysiaFramework;
using ElysiaFramework.Interfaces;
using ElysiaFramework.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StickyHomeworks.Services;

namespace StickyHomeworks;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : AppEx
{
    protected override void OnStartup(StartupEventArgs e)
    {
        Host = Microsoft.Extensions.Hosting.Host.
            CreateDefaultBuilder().
            UseContentRoot(AppContext.BaseDirectory).
            ConfigureServices((context, services) =>
            {
                services.AddSingleton<IThemeService, ThemeService>();
                services.AddSingleton<ProfileService>();
                services.AddSingleton<SettingsService>();
                services.AddSingleton<MainWindow>();
            }).
            Build();
        _ = Host.StartAsync();
        GetService<MainWindow>().Show();
        base.OnStartup(e);
    }
}