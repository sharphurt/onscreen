using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using onscreen.Controls;

namespace onscreen.API;

public static class Util
{
    public static bool IsClickedToExistingControl(DrawingCanvas canvas, Point position)
    {
        return canvas.Children.Cast<Control>().Any(c => GetControlBounds(c).Contains(position));
    }

    public static Control GetControlUnderCursor(DrawingCanvas canvas, DrawingProperties properties)
    {
        var controls = canvas.Children
            .Cast<Control>()
            .Where(c => GetControlBounds(c).Contains(properties.Position))
            .ToList();

        return controls.Any() ? controls.First() : null;
    }

    public static Rect GetControlBounds(Control control)
    {
        return new Rect(control.Margin.Left - 5, control.Margin.Top - 5, control.ActualWidth + 5, control.ActualHeight + 5);
    }

    public static bool ReachedMinimalSize(Control control)
    {
        return Math.Abs(control.Width - control.MinWidth) < 1e-1 && Math.Abs(control.Height - control.MinHeight) < 1e-1;
    }
    public static bool IsUserVisible(FrameworkElement element, FrameworkElement container)
    {
        if (!element.IsVisible)
            return false;

        Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
        Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
        return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
    }
    
}