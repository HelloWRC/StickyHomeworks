﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;
using StickyHomeworks.Services;
using StickyHomeworks.ViewModels;

namespace StickyHomeworks.Views;

/// <summary>
/// HomeworkEditWindow.xaml 的交互逻辑
/// </summary>
public partial class HomeworkEditWindow : Window
{
    private RichTextBox _relatedRichTextBox = new();
    public MainWindow MainWindow { get; }
    public SettingsService SettingsService { get; }

    public HomeworkEditViewModel ViewModel { get; } = new();

    public RichTextBox RelatedRichTextBox
    {
        get => _relatedRichTextBox;
        set
        {
            UnregisterOldTextBox(_relatedRichTextBox);
            RegisterNewTextBox(value);
            _relatedRichTextBox = value;
        }
    }

    public HomeworkEditWindow(MainWindow mainWindow, SettingsService settingsService)
    {
        MainWindow = mainWindow;
        SettingsService = settingsService;
        DataContext = this;
        InitializeComponent();
        ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
    }

    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var s = RelatedRichTextBox.Selection;
        if (e.PropertyName == nameof(ViewModel.TextColor) && !ViewModel.IsRestoringSelection)
        {
            s.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(ViewModel.TextColor));
        }
    }

    private void RegisterNewTextBox(RichTextBox richTextBox)
    {
        richTextBox.TextChanged += RichTextBoxOnTextChanged;
        richTextBox.SelectionChanged += RichTextBoxOnSelectionChanged;
    }

    private void RichTextBoxOnSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (ViewModel.IsRestoringSelection)
            return;
        Debug.WriteLine("selection changed!");
        if (RelatedRichTextBox.Selection.Start.Paragraph != null)
            ViewModel.SelectedParagraph = RelatedRichTextBox.Selection.Start.Paragraph;
        // Update selection
        var s = RelatedRichTextBox.Selection;
        if (!MainWindow.IsActive)
        {
            if (ViewModel is not { BeforeTextPointerStart: not null, BeforeTextPointerEnd: not null })
                return;
            ViewModel.IsRestoringSelection = true;
            RelatedRichTextBox.Selection.Select(ViewModel.BeforeTextPointerStart, ViewModel.BeforeTextPointerEnd);
            ViewModel.IsRestoringSelection = false;
            return;
        }
        ViewModel.Selection = s;
        ViewModel.BeforeTextPointerStart = s.Start;
        ViewModel.BeforeTextPointerEnd = s.End;
        ViewModel.IsRestoringSelection = true;
        Debug.WriteLine("selection updated!");
        var w = s.GetPropertyValue(TextElement.FontWeightProperty);
        if (w is FontWeight weight)
        {
            ViewModel.IsBold = weight >= FontWeights.Bold;
        }

        ViewModel.IsItalic = Equals(s.GetPropertyValue(TextElement.FontStyleProperty), FontStyles.Italic);
        if (s.GetPropertyValue(Paragraph.TextDecorationsProperty) is TextDecorationCollection decorations)
        {
            ViewModel.IsUnderlined = decorations.Contains(TextDecorations.Underline[0]);
            ViewModel.IsStrikeThrough = decorations.Contains(TextDecorations.Strikethrough[0]);
        }

        if (s.GetPropertyValue(TextElement.ForegroundProperty) is SolidColorBrush fg)
        {
            ViewModel.TextColor = fg.Color;
        }
        ViewModel.IsRestoringSelection = false;
    }

    private void RichTextBoxOnTextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void UnregisterOldTextBox(RichTextBox richTextBox)
    {
        richTextBox.TextChanged -= RichTextBoxOnTextChanged;
        richTextBox.SelectionChanged -= RichTextBoxOnSelectionChanged;
    }

    private void ListBoxTextStyles_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var s = RelatedRichTextBox.Selection;
        s.ApplyPropertyValue(TextElement.FontWeightProperty, ViewModel.IsBold? FontWeights.Bold : FontWeights.Regular);
        s.ApplyPropertyValue(TextElement.FontStyleProperty, ViewModel.IsItalic ? FontStyles.Italic : FontStyles.Normal);
        var decorations = new TextDecorationCollection();
        if (ViewModel.IsUnderlined)
            decorations.Add(TextDecorations.Underline);
        if (ViewModel.IsStrikeThrough)
            decorations.Add(TextDecorations.Strikethrough);
        s.ApplyPropertyValue(Paragraph.TextDecorationsProperty, decorations);
        RelatedRichTextBox.Focus();
    }

    private void ButtonClearColor_OnClick(object sender, RoutedEventArgs e)
    {
        var s = RelatedRichTextBox.Selection;
        s.ApplyPropertyValue(TextElement.ForegroundProperty, GetValue(TextElement.ForegroundProperty));
    }
}