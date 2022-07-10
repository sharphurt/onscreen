using System;
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

    public void Process(DrawingCanvas sender, DrawingProperties properties)
    {
        sender.EditingMode = InkCanvasEditingMode.None;

        var textField = new TextField
        {
            Margin = new Thickness(properties.Position.X, properties.Position.Y, 0, 0),
            Width = 100,
            Height = 50
        };
        sender.Children.Add(textField);
    }

}