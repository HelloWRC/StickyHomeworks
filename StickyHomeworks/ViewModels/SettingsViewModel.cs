using CommunityToolkit.Mvvm.ComponentModel;

namespace StickyHomeworks.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private int _appIconClickCount = 0;
    private object? _drawerContent;
    private bool _isPopupMenuOpened;
    private string _license = "";

    public int AppIconClickCount
    {
        get => _appIconClickCount;
        set
        {
            if (value == _appIconClickCount) return;
            _appIconClickCount = value;
            OnPropertyChanged();
        }
    }

    public object? DrawerContent
    {
        get => _drawerContent;
        set
        {
            if (Equals(value, _drawerContent)) return;
            _drawerContent = value;
            OnPropertyChanged();
        }
    }

    public bool IsPopupMenuOpened
    {
        get => _isPopupMenuOpened;
        set
        {
            if (value == _isPopupMenuOpened) return;
            _isPopupMenuOpened = value;
            OnPropertyChanged();
        }
    }

    public string License
    {
        get => _license;
        set
        {
            if (value == _license) return;
            _license = value;
            OnPropertyChanged();
        }
    }
}