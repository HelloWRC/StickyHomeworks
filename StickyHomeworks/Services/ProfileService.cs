using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using StickyHomeworks.Models;

namespace StickyHomeworks.Services;

public class ProfileService : IHostedService, INotifyPropertyChanged
{
    private Profile _profile = new();

    public ProfileService(IHostApplicationLifetime applicationLifetime)
    {
        LoadProfile();
        //applicationLifetime.ApplicationStopping.Register(SaveProfile);
        Profile.PropertyChanged += (sender, args) => SaveProfile();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }

    public void LoadProfile()
    {
        if (!File.Exists("./Profile.json"))
        {
            return;
        }
        var json = File.ReadAllText("./Profile.json");
        var r = JsonSerializer.Deserialize<Profile>(json);
        if (r != null)
        {
            Profile = r;
            Profile.PropertyChanged += (sender, args) => SaveProfile();
        }
    }

    public void SaveProfile()
    {
        File.WriteAllText("./Profile.json", JsonSerializer.Serialize<Profile>(Profile));
    }

    public Profile Profile
    {
        get => _profile;
        set
        {
            if (Equals(value, _profile)) return;
            _profile = value;
            OnPropertyChanged();
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}