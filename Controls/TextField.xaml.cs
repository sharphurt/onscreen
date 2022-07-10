using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using onscreen.API;

namespace onscreen.Controls;

public partial class TextField : UserControl
{
    public TextField()
    {
        InitializeComponent();
    }

    public bool IsSingleLine { get; set; } = true;

    private void TopLeftControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            IsSingleLine = false;
            
            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
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
            IsSingleLine = false;

            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
            var bottomLeft = Util.GetControlBounds(this).BottomLeft;

            var newWidht = Math.Abs(bottomLeft.X - mousePosition.X);
            var newHeight = Math.Abs(bottomLeft.Y - mousePosition.Y);

            Margin = new Thickness(Margin.Left, mousePosition.Y, 0, 0);
            Width = newWidht;
            Height = newHeight;

            e.Handled = true;
        }
    }

    private void BottomRightControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            IsSingleLine = false;

            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
            var topLeft = Util.GetControlBounds(this).TopLeft;

            var newWidht = Math.Abs(topLeft.X - mousePosition.X);
            var newHeight = Math.Abs(topLeft.Y - mousePosition.Y);

            Width = newWidht;
            Height = newHeight;

            e.Handled = true;
        }
    }

    private void BottomLeftControl_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            IsSingleLine = false;

            var mousePosition = e.GetPosition((DrawingCanvas)Parent);
            var topRight = Util.GetControlBounds(this).TopRight;

            var newWidht = Math.Abs(topRight.X - mousePosition.X);
            var newHeight = Math.Abs(topRight.Y - mousePosition.Y);

            Margin = new Thickness(mousePosition.X, Margin.Top, 0, 0);

            Width = newWidht;
            Height = newHeight;

            e.Handled = true;
        }
    }

    private void TextField_OnLostFocus(object sender, RoutedEventArgs e)
    {
        TextBox.BorderThickness = new Thickness(0);
        SizeControls.Visibility = Visibility.Hidden;
    }

    private void TextField_OnGotFocus(object sender, RoutedEventArgs e)
    {
        TextBox.BorderThickness = new Thickness(1);
        SizeControls.Visibility = Visibility.Visible;
    }
}