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

        textField.DeleteEvent += () => canvas.Children.Remove(textField);

        canvas.Children.Add(textField);
        textField.Focus();

        return textField;
    }

    public void ProcessResizing(DrawingCanvas canvas, DrawingProperties properties)
    {
        var cursorPosition = properties.Position;
        var control = (TextField)properties.Control;

        var controlX = control.Margin.Left;
        var controlY = control.Margin.Top + 50;

        control.ContentContainer.Width = Math.Abs(cursorPosition.X - controlX);
        control.ContentContainer.Height = Math.Abs(cursorPosition.Y - controlY);
    }

    public static void RemoveAllEmpty(DrawingCanvas canvas)
    {
        var allEmpty = (from child in canvas.Children.OfType<TextField>()
            let text = child.TextBox.Text
            where string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)
            select child).ToList();

        allEmpty.ForEach(o => canvas.Children.Remove(o));
    }
}