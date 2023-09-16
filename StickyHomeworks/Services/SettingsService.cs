using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Hosting;
using StickyHomeworks.Models;

namespace StickyHomeworks.Services;

public class SettingsService : ObservableRecipient, IHostedService
{
    private Settings _settings = new();

    public async Task StartAsync(CancellationToken cancellationToken)
    {
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }

    public Settings Settings
    {
        get => _settings;
        set
        {
            if (Equals(value, _settings)) return;
            _settings = value;
            OnPropertyChanged();
        }
    }
}