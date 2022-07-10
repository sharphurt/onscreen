using System;
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
        Console.WriteLine("Text drawing   " + properties.Position);
    }
}