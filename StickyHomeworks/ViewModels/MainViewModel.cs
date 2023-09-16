using CommunityToolkit.Mvvm.ComponentModel;
using StickyHomeworks.Models;

namespace StickyHomeworks.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private bool _isDrawerOpened = false;
    private Homework? _selectedHomework;
    private Homework _editingHomework = new();

    public bool IsDrawerOpened
    {
        get => _isDrawerOpened;
        set
        {
            if (value == _isDrawerOpened) return;
            _isDrawerOpened = value;
            OnPropertyChanged();
        }
    }

    public Homework? SelectedHomework
    {
        get => _selectedHomework;
        set
        {
            if (Equals(value, _selectedHomework)) return;
            _selectedHomework = value;
            OnPropertyChanged();
        }
    }

    public Homework EditingHomework
    {
        get => _editingHomework;
        set
        {
            if (Equals(value, _editingHomework)) return;
            _editingHomework = value;
            OnPropertyChanged();
        }
    }
}