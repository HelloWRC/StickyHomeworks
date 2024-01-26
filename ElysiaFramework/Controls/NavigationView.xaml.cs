using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElysiaFramework.Controls;
/// <summary>
/// NavigationView.xaml 的交互逻辑
/// </summary>
public partial class NavigationView : UserControl
{
    public static readonly DependencyProperty NavigatedContentProperty = DependencyProperty.Register(
        nameof(NavigatedContent), typeof(object), typeof(NavigationView), new PropertyMetadata(default(object)));

    public object NavigatedContent
    {
        get => (object)GetValue(NavigatedContentProperty);
        set => SetValue(NavigatedContentProperty, value);
    }

    public static readonly DependencyProperty NavigationItemsProperty = DependencyProperty.Register(
        nameof(NavigationItems), typeof(Collection<object>), typeof(NavigationView), new PropertyMetadata(default(Collection<object>)));

    public Collection<object> NavigationItems
    {
        get => (Collection<object>)GetValue(NavigationItemsProperty);
        set => SetValue(NavigationItemsProperty, value);
    }

    public static readonly DependencyProperty NavigationItemTemplateProperty = DependencyProperty.Register(
        nameof(NavigationItemTemplate), typeof(DataTemplate), typeof(NavigationView), new PropertyMetadata(default(DataTemplate)));

    public DataTemplate NavigationItemTemplate
    {
        get => (DataTemplate)GetValue(NavigationItemTemplateProperty);
        set => SetValue(NavigationItemTemplateProperty, value);
    }

    public static readonly DependencyProperty BottomNavigationItemsProperty = DependencyProperty.Register(
        nameof(BottomNavigationItems), typeof(Collection<object>), typeof(NavigationView), new PropertyMetadata(default(Collection<object>)));

    public Collection<object> BottomNavigationItems
    {
        get => (Collection<object>)GetValue(BottomNavigationItemsProperty);
        set => SetValue(BottomNavigationItemsProperty, value);
    }

    public NavigationView()
    {
        InitializeComponent();
    }
}
