using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElysiaFramework;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors;
using StickyHomeworks.Behaviors;
using StickyHomeworks.Models;
using StickyHomeworks.Services;
using StickyHomeworks.ViewModels;
using StickyHomeworks.Views;
using System.Windows.Automation;

namespace StickyHomeworks;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; set; } = new MainViewModel();

    public ProfileService ProfileService { get; }

    public SettingsService SettingsService { get; }

    public event EventHandler? OnHomeworkEditorUpdated;

    public MainWindow(ProfileService profileService,
                      SettingsService settingsService)
    {
        ProfileService = profileService;
        SettingsService = settingsService;
        Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);
        InitializeComponent();
        ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        DataContext = this;
    }

    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.SelectedListBoxItem))
        {
            RepositionEditingWindow();
        }
    }

    private void OnFocusChangedHandler(object sender, AutomationFocusChangedEventArgs e)
    {
        try
        {

            var element = sender as AutomationElement;
            if (element == null)
                return;
            var hWnd = NativeWindowHelper.GetForegroundWindow();
            NativeWindowHelper.GetWindowThreadProcessId(hWnd, out var id);
            using var proc = Process.GetProcessById(id);
            Debug.WriteLine($"{proc.ProcessName} {e.EventId.ProgrammaticName}");
            if (proc.Id != Environment.ProcessId &&
                !new List<string>(["ctfmon", "textinputhost", "chsime"]).Contains(proc.ProcessName.ToLower()))
            {
                Dispatcher.Invoke(ExitEditingMode);
            }
        }
        catch
        {
            // ignored
        }
    }

    private void ExitEditingMode()
    {
        MainListView.SelectedIndex = -1;
        ViewModel.IsDrawerOpened = false;
        AppEx.GetService<HomeworkEditWindow>().TryClose();
    }

    private void SetPos()
    {
        GetCurrentDpi(out var dpi, out _);
        Left = SettingsService.Settings.WindowX / dpi;
        Top = SettingsService.Settings.WindowY / dpi;
        Width = SettingsService.Settings.WindowWidth / dpi;
        Height = SettingsService.Settings.WindowHeight / dpi;
    }

    private void SavePos()
    {
        GetCurrentDpi(out var dpi, out _);
        SettingsService.Settings.WindowX = Left * dpi;
        SettingsService.Settings.WindowY = Top * dpi;
        if (ViewModel.IsExpanded)
        {
            SettingsService.Settings.WindowWidth = Width * dpi;
            SettingsService.Settings.WindowHeight = Height * dpi;
        }
    }

    protected override void OnInitialized(EventArgs e)
    {
        var rm = ProfileService.CleanupOutdated();
        if (rm.Count > 0)
        {
            ViewModel.SnackbarMessageQueue.Enqueue($"清除了{rm.Count}条过期的作业。",
                "恢复", (o) =>
                {
                    foreach (var i in rm)
                    {
                        ProfileService.Profile.Homeworks.Add(i);
                    }
                }, null, false, false, TimeSpan.FromSeconds(30));
        }
        base.OnInitialized(e);
    }

    protected override void OnContentRendered(EventArgs e)
    {
        SetBottom();
        SetPos();
        AppEx.GetService<HomeworkEditWindow>().EditingFinished += OnEditingFinished;
        AppEx.GetService<HomeworkEditWindow>().SubjectChanged += OnSubjectChanged;
        base.OnContentRendered(e);
    }

    private void OnSubjectChanged(object? sender, EventArgs e)
    {
        if (ViewModel.IsUpdatingHomeworkSubject)
            return;
        if (ViewModel.SelectedHomework == null)
            return;
        ViewModel.IsUpdatingHomeworkSubject = true;
        var s = ViewModel.SelectedHomework;
        ProfileService.Profile.Homeworks.Remove(s);
        ProfileService.Profile.Homeworks.Add(s);
        ViewModel.SelectedHomework = s;
        ViewModel.IsUpdatingHomeworkSubject = false;
    }

    private void OnEditingFinished(object? sender, EventArgs e)
    {
        ExitEditingMode();
    }

    private void ButtonCreateHomework_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.IsUpdatingHomeworkSubject = true;
        OnHomeworkEditorUpdated?.Invoke(this ,EventArgs.Empty);
        var lastSubject = ViewModel.EditingHomework.Subject;
        //ViewModel.IsCreatingMode = true;
        ViewModel.IsDrawerOpened = true;
        var o = new Homework()
        {
            Subject = lastSubject
        };
        ViewModel.EditingHomework = o;
        ViewModel.SelectedHomework = o;
        ProfileService.Profile.Homeworks.Add(o);
        ComboBoxSubject.Text = lastSubject;
        SettingsService.SaveSettings();
        ProfileService.SaveProfile();
        ViewModel.IsUpdatingHomeworkSubject = false;
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
        if (!ViewModel.IsClosing)
        {
            e.Cancel = true;
            return;
        }

        SavePos();
        SettingsService.SaveSettings();
        ProfileService.SaveProfile();
    }

    private void ButtonEditTags_OnClick(object sender, RoutedEventArgs e)
    {
        OnHomeworkEditorUpdated?.Invoke(this, EventArgs.Empty);
        ViewModel.IsTagEditingPopupOpened = true;
    }

    private void ButtonEditHomework_OnClick(object sender, RoutedEventArgs e)
    {
        OnHomeworkEditorUpdated?.Invoke(this, EventArgs.Empty);
        ViewModel.IsCreatingMode = false;
        if (ViewModel.SelectedHomework== null)
            return;
        ViewModel.EditingHomework = ViewModel.SelectedHomework;
        ViewModel.IsDrawerOpened = true;
        RepositionEditingWindow();
        AppEx.GetService<HomeworkEditWindow>().TryOpen();
    }

    private void RepositionEditingWindow()
    {
        if (ViewModel.SelectedListBoxItem != null)
        {
            Debug.WriteLine("selected changed");
            GetCurrentDpi(out var dpiX, out var dpiY);
            var p = ViewModel.SelectedListBoxItem.PointToScreen(new Point(ViewModel.SelectedListBoxItem.ActualWidth, 0));
            AppEx.GetService<HomeworkEditWindow>().Left = p.X / dpiX;
            AppEx.GetService<HomeworkEditWindow>().Top = p.Y / dpiY;
        }
    }

    private void ButtonRemoveHomework_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.IsUpdatingHomeworkSubject = true;
        if (ViewModel.SelectedHomework == null)
            return;
        ProfileService.Profile.Homeworks.Remove(ViewModel.SelectedHomework);
        ViewModel.IsUpdatingHomeworkSubject = false;
    }

    private void ButtonEditDone_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.IsDrawerOpened = false;
    }

    private void DragBorder_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (ViewModel.IsUnlocked && e.LeftButton == MouseButtonState.Pressed)
        {
            SetBottom();
            DragMove();
            SetBottom();
        }
    }

    private void SetBottom()
    {
        if (!SettingsService.Settings.IsBottom)
        {
            return;
        }
        var hWnd = new WindowInteropHelper(this).Handle;
        NativeWindowHelper.SetWindowPos(hWnd, NativeWindowHelper.HWND_BOTTOM, 0, 0, 0, 0, NativeWindowHelper.SWP_NOSIZE | NativeWindowHelper.SWP_NOMOVE | NativeWindowHelper.SWP_NOACTIVATE);
    }

    private void MainWindow_OnStateChanged(object? sender, EventArgs e)
    {
        SetBottom();
    }

    private void MainWindow_OnActivated(object? sender, EventArgs e)
    {
        SetBottom();
    }

    private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.IsClosing = true;
        Close();
    }

    private void ButtonDateSetToday_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.EditingHomework.DueTime = DateTime.Today;
    }

    private void ButtonDateSetWeekends_OnClick(object sender, RoutedEventArgs e)
    {
        var today = DateTime.Today;
        var delta = DayOfWeek.Saturday - today.DayOfWeek + 1;
        ViewModel.EditingHomework.DueTime = today + TimeSpan.FromDays(delta);
    }

    private void ButtonExpandingSwitcher_OnClick(object sender, RoutedEventArgs e)
    {
        SavePos();
        ViewModel.IsExpanded = !ViewModel.IsExpanded;
        if (ViewModel.IsExpanded)
        {
            SizeToContent = SizeToContent.Manual;
            SetPos();
        }
        else
        {
            ViewModel.IsUnlocked = false;
            SizeToContent = SizeToContent.Height;
            Width = Math.Min(ActualWidth, 350);
        }
    }

    private void MainWindow_OnDeactivated(object? sender, EventArgs e)
    {
        //MainListView.SelectedIndex = -1;
    }

    private async void ButtonExport_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.IsWorking = true;
        var dialog = new System.Windows.Forms.SaveFileDialog()
        {
            Filter = "文本文件 (*.txt)|*.txt"
        };
        if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        {
            goto done;
        }

        var file = dialog.FileName!;
        try
        {
            await Task.Run(() =>
            {
                var h = from i in ProfileService.Profile.Homeworks
                    orderby i.Subject
                    select i;
                string? lastSubject = null;
                var outText = new List<string>();
                foreach (var i in h)
                {
                    if (lastSubject == null || i.Subject != lastSubject)
                    {
                        lastSubject = i.Subject;
                        outText.Add(i.Subject);
                    }

                    outText.Add($"- {i.Content} {string.Join(' ', from t in i.Tags select $"【{t}】")}");
                }

                File.WriteAllText(file, string.Join('\n', outText));
                ViewModel.SnackbarMessageQueue.Enqueue($"成功地导出到：{file}", "查看", () =>
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = file,
                        UseShellExecute = true
                    });
                });

            });
        }
        catch(Exception ex)
        {
            ViewModel.SnackbarMessageQueue.Enqueue($"导出失败：{ex}");
        }

        done: 
        dialog.Dispose();
        ViewModel.IsWorking = false;
    }

    private void DrawerHost_OnDrawerClosing(object? sender, DrawerClosingEventArgs e)
    {
        SettingsService.SaveSettings();
        ProfileService.SaveProfile();
    }

    private void ButtonMore_Click(object sender, RoutedEventArgs e)
    {
        PopupExAdvanced.IsOpen = true;
    }

    private void MainListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RepositionEditingWindow();
    }
}