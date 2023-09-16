using CommunityToolkit.Mvvm.ComponentModel;
using StickyHomeworks.Models;

namespace StickyHomeworks.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private bool _isDrawerOpened = false;
    private Homework? _selectedHomework;
    private Homework _editingHomework = new();
    private bool _isTagEditingPopupOpened = false;
    private bool _isCreatingMode = false;
    private bool _isUnlocked = false;
    private bool _isExpanded = true;

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

    public bool IsTagEditingPopupOpened
    {
        get => _isTagEditingPopupOpened;
        set
        {
            if (value == _isTagEditingPopupOpened) return;
            _isTagEditingPopupOpened = value;
            OnPropertyChanged();
        }
    }

    public bool IsCreatingMode
    {
        get => _isCreatingMode;
        set
        {
            if (value == _isCreatingMode) return;
            _isCreatingMode = value;
            OnPropertyChanged();
        }
    }

    public bool IsUnlocked
    {
        get => _isUnlocked;
        set
        {
            if (value == _isUnlocked) return;
            _isUnlocked = value;
            OnPropertyChanged();
        }
    }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (value == _isExpanded) return;
            _isExpanded = value;
            OnPropertyChanged();
        }
    }
}