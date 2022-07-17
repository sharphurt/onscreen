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

    public void Initialize(DrawingCanvas canvas)
    {
        canvas.InkCanvas.EditingMode = InkCanvasEditingMode.None;
    }

    public Control ProcessCreating(DrawingCanvas canvas, DrawingProperties properties)
    {
        var textField = new TextField();

        textField.Margin = new Thickness(
            properties.Position.X - textField.MinWidth,
            properties.Position.Y - textField.MinHeight, 0, 0);

        canvas.Children.Add(textField);

        textField.Focus();

        return textField;
    }

    public void ProcessResizing(DrawingCanvas canvas, DrawingProperties properties)
    {
        var cursorPosition = properties.Position;
        var control = properties.Control;

        var controlX = control.Margin.Left;
        var controlY = control.Margin.Top;

        control.Width = Math.Abs(cursorPosition.X - controlX);
        control.Height = Math.Abs(cursorPosition.Y - controlY);
    }
}