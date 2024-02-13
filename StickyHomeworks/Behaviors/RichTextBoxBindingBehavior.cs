using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace StickyHomeworks.Behaviors;

public class RichTextBoxBindingBehavior : Behavior<RichTextBox>
{
    public static readonly DependencyProperty TextXamlProperty = DependencyProperty.Register(
        nameof(TextXaml), typeof(string), typeof(RichTextBoxBindingBehavior), new PropertyMetadata(""));

    public string TextXaml
    {
        get { return (string)GetValue(TextXamlProperty); }
        set { SetValue(TextXamlProperty, value); }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
    }

    private void AssociatedObjectOnTextChanged(object sender, TextChangedEventArgs e)
    {
        
    }
}