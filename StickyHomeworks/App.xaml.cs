using System.Configuration;
using System.Data;
using System.Windows;
using ElysiaFramework;
using Microsoft.Extensions.Hosting;

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
            ConfigureServices((context, services) => { }).
            Build();
        _ = Host.StartAsync();
        base.OnStartup(e);
    }
}