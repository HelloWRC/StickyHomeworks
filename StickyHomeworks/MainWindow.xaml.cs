using System.ComponentModel;
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
using ElysiaFramework;
using StickyHomeworks.Models;
using StickyHomeworks.Services;
using StickyHomeworks.ViewModels;
using StickyHomeworks.Views;

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
            
        };
        ViewModel.EditingHomework = o;
    }

    private void ButtonAddHomeworkCompleted_OnClick(object sender, RoutedEventArgs e)
    {
        ProfileService.Profile.Homeworks.Add(ViewModel.EditingHomework);
        ViewModel.IsDrawerOpened = false;
    }

    public void GetCurrentDpi(out double dpiX, out double dpiY)
    {
        var source = PresentationSource.FromVisual(this);

        dpiX = 1.0;
        dpiY = 1.0;

        if (source?.CompositionTarget != null)
        {
            dpiX = 1.0 * source.CompositionTarget.TransformToDevice.M11;
            dpiY = 1.0 * source.CompositionTarget.TransformToDevice.M22;
        }
    }

    private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
    {
        OpenSettingsWindow();
    }

    private void OpenSettingsWindow()
    {
        var win = AppEx.GetService<SettingsWindow>();
        if (!win.IsOpened)
        {
            //Analytics.TrackEvent("打开设置窗口");
            win.IsOpened = true;
            win.Show();
        }
        else
        {
            if (win.WindowState == WindowState.Minimized)
            {
                win.WindowState = WindowState.Normal;
            }

            win.Activate();
        }
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        SettingsService.SaveSettings();
        ProfileService.SaveProfile();
    }
}