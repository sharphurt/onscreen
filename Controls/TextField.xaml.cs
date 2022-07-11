using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HandyControl.Tools.Extension;
using onscreen.API;

namespace onscreen.Controls;

public partial class TextField : UserControl
{
    public TextField()
    {
        InitializeComponent();
    }

    private void TopLeftControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
            mousePosition.Y -= TextFormatingPanel.ActualHeight + 3;
            mousePosition.X -= 3;
            var bottomRight = Util.GetControlBounds(this).BottomRight;

            var newWidht = Math.Abs(bottomRight.X - mousePosition.X);
            var newHeight = Math.Abs(bottomRight.Y - mousePosition.Y);

            Margin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
            Width = newWidht;
            Height = newHeight;

            e.Handled = true;
        }
    }

    private void TopRightControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
            mousePosition.Y -= TextFormatingPanel.ActualHeight + 3;
            mousePosition.X -= 3;
            var bottomLeft = Util.GetControlBounds(this).BottomLeft;

            var newWidht = Math.Abs(bottomLeft.X - mousePosition.X);
            var newHeight = Math.Abs(bottomLeft.Y - mousePosition.Y);

            Margin = new Thickness(Margin.Left, mousePosition.Y, 0, 0);
            Width = newWidht;
            Height = newHeight;
        }

        e.Handled = true;
    }

    private void BottomRightControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
            var topLeft = Util.GetControlBounds(this).TopLeft;

            var newWidht = Math.Abs(topLeft.X - mousePosition.X);
            var newHeight = Math.Abs(topLeft.Y - mousePosition.Y);

            Width = newWidht;
            Height = newHeight;
        }

        e.Handled = true;
    }

    private void BottomLeftControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
            var topRight = Util.GetControlBounds(this).TopRight;

            var newWidht = Math.Abs(topRight.X - mousePosition.X);
            var newHeight = Math.Abs(topRight.Y - mousePosition.Y);

            Margin = new Thickness(mousePosition.X, Margin.Top, 0, 0);

            Width = newWidht;
            Height = newHeight;
        }

        e.Handled = true;
    }

    private void PositionControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
            Margin = new Thickness(
                mousePosition.X - TextBox.ActualWidth / 2,
                mousePosition.Y - TextBox.ActualHeight / 2,
                0, 0);
        }

        e.Handled = true;
    }

    private void TextField_OnLostFocus(object sender, RoutedEventArgs e)
    {
        TextBox.BorderThickness = new Thickness(0);
        TransformControls.Visibility = Visibility.Hidden;
        TextFormatingPanel.Visibility = Visibility.Collapsed;
        Margin = new Thickness(Margin.Left, Margin.Top + 36, 0, 0);
    }

    private void TextField_OnGotFocus(object sender, RoutedEventArgs e)
    {
        TextBox.BorderThickness = new Thickness(1);
        TransformControls.Visibility = Visibility.Visible;
        TextFormatingPanel.Visibility = Visibility.Visible;
        Margin = new Thickness(Margin.Left, Margin.Top - 36, 0, 0);
    }

    private void TextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        object temp = TextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
        ButtonBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

        temp = TextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
        ButtonItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

        temp = TextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
        ButtonUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

        /*
        temp = TextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
        cmbFontFamily.SelectedItem = temp;
        temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
        cmbFontSize.Text = temp.ToString(); */
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var comboBox = ((ComboBox)sender);
        Console.WriteLine(((ComboBoxItem)comboBox.SelectedItem).Content);
        comboBox.FontFamily = new FontFamily((string)((ComboBoxItem)comboBox.SelectedItem).Content);
        comboBox.FontSize = ((ComboBoxItem)comboBox.SelectedItem).FontSize;
    }
}