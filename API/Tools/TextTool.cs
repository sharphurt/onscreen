using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using onscreen.Controls;

namespace onscreen.API.Tools;

public class TextTool : ITool
{
    public ToolType ToolType { get; set; }

    public TextTool()
    {
        ToolType = ToolType.Text;
    }

    public Control ProcessCreating(DrawingCanvas canvas, DrawingProperties properties)
    {
        canvas.EditingMode = InkCanvasEditingMode.None;

        if (!IsClickedToExistingTextField(canvas, properties))
        {
            var textField = new TextField
            {
                Margin = new Thickness(properties.Position.X, properties.Position.Y, 0, 0),
                Width = 100,
                Height = 50
            };
            canvas.Children.Add(textField);
            return textField;
        }

        return null;
    }

    public void ProcessResizing(DrawingCanvas canvas, DrawingProperties properties)
    {
        var cursorPosition = properties.Position;
        var controlX = properties.Control.Margin.Left;
        var controlY = properties.Control.Margin.Top;
        
        properties.Control.Width = Math.Abs(cursorPosition.X - controlX);
        properties.Control.Height = Math.Abs(cursorPosition.Y - controlY);
    }

    private bool IsClickedToExistingTextField(DrawingCanvas canvas, DrawingProperties properties)
    {
        return canvas.Children.Cast<Control>().Any(c =>
            new Rect(c.Margin.Left, c.Margin.Top, c.Width, c.Height).Contains(properties.Position));
    }

    private Control GetControlUnderCursor(DrawingCanvas canvas, DrawingProperties properties)
    {
        var controls = canvas.Children.Cast<Control>().Where(c =>
            new Rect(c.Margin.Left, c.Margin.Top, c.Width, c.Height).Contains(properties.Position)).ToList();

        return controls.Any() ? controls.First() : null;
    }
}