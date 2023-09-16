using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StickyHomeworks.Models;
using StickyHomeworks.Services;
using StickyHomeworks.ViewModels;

namespace StickyHomeworks;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; set; } = new MainViewModel();

    public ProfileService ProfileService { get; }

    public SettingsService SettingsService { get; }

    public MainWindow(ProfileService profileService,
                      SettingsService settingsService)
    {
        ProfileService = profileService;
        SettingsService = settingsService;

        InitializeComponent();
        DataContext = this;
    }

    private void ButtonCreateHomework_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.IsDrawerOpened = true;
        var o = new Homework()
        {
            Tags = {"test", "123", "456", "444"},
            Subject = "test"
        };
        ProfileService.Profile.Homeworks.Add(o);
        ViewModel.SelectedHomework = o;
    }
}