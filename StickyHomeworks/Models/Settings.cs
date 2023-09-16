using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignColors;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace StickyHomeworks.Models;

public class Settings : ObservableRecipient
{
    private int _selectedPlatteIndex = 0;
    private int _theme = 0;
    private Color _primaryColor = Color.FromRgb(34, 209, 236);
    private Color _secondaryColor  = Color.FromRgb(34, 209, 236);
    private int _colorSource = 1;
    private ObservableCollection<Color> _wallpaperColorPlatte = new();
    private bool _isWallpaperAutoUpdateEnabled = false;
    private int _wallpaperAutoUpdateIntervalSeconds = 60;
    private string _wallpaperClassName = "";
    private bool _isFallbackModeEnabled = true;
    private double _targetLightValue = 0.6;
    private double _opacity = 0.7;
    private double _scale = 1.5;

    #region Appearence

    public int Theme
    {
        get => _theme;
        set
        {
            if (value == _theme) return;
            _theme = value;
            OnPropertyChanged();
        }
    }

    public Color PrimaryColor
    {
        get => _primaryColor;
        set
        {
            if (value.Equals(_primaryColor)) return;
            _primaryColor = value;
            OnPropertyChanged();
        }
    }

    public Color SecondaryColor
    {
        get => _secondaryColor;
        set
        {
            if (value.Equals(_secondaryColor)) return;
            _secondaryColor = value;
            OnPropertyChanged();
        }
    }

    public int ColorSource
    {
        get => _colorSource;
        set
        {
            if (value == _colorSource) return;
            _colorSource = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Color> WallpaperColorPlatte
    {
        get => _wallpaperColorPlatte;
        set
        {
            if (Equals(value, _wallpaperColorPlatte)) return;
            _wallpaperColorPlatte = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public Color SelectedPlatte => WallpaperColorPlatte.Count < Math.Max(SelectedPlatteIndex, 0) + 1
        ? Colors.DodgerBlue
        : WallpaperColorPlatte[SelectedPlatteIndex];

    public int SelectedPlatteIndex
    {
        get => _selectedPlatteIndex;
        set
        {
            if (value == _selectedPlatteIndex) return;
            _selectedPlatteIndex = value;
            OnPropertyChanged();
        }
    }

    public bool IsWallpaperAutoUpdateEnabled
    {
        get => _isWallpaperAutoUpdateEnabled;
        set
        {
            if (value == _isWallpaperAutoUpdateEnabled) return;
            _isWallpaperAutoUpdateEnabled = value;
            OnPropertyChanged();
        }
    }

    public int WallpaperAutoUpdateIntervalSeconds
    {
        get => _wallpaperAutoUpdateIntervalSeconds;
        set
        {
            if (value == _wallpaperAutoUpdateIntervalSeconds) return;
            _wallpaperAutoUpdateIntervalSeconds = value;
            OnPropertyChanged();
        }
    }

    public string WallpaperClassName
    {
        get => _wallpaperClassName;
        set
        {
            if (value == _wallpaperClassName) return;
            _wallpaperClassName = value;
            OnPropertyChanged();
        }
    }

    public bool IsFallbackModeEnabled
    {
        get => _isFallbackModeEnabled;
        set
        {
            if (value == _isFallbackModeEnabled) return;
            _isFallbackModeEnabled = value;
            OnPropertyChanged();
        }
    }

    public double TargetLightValue
    {
        get => _targetLightValue;
        set
        {
            if (value.Equals(_targetLightValue)) return;
            _targetLightValue = value;
            OnPropertyChanged();
        }
    }

    public double Opacity
    {
        get => _opacity;
        set
        {
            if (value.Equals(_opacity)) return;
            _opacity = value;
            OnPropertyChanged();
        }
    }

    public double Scale
    {
        get => _scale;
        set
        {
            if (value.Equals(_scale)) return;
            _scale = value;
            OnPropertyChanged();
        }
    }

    #endregion
}