using System;
using System.Windows.Controls;
using onscreen.Controls;

namespace onscreen.API.Tools
{
    public class PenTool : ITool
    {
        public ToolType ToolType { get; set; }

        public void Process(DrawingCanvas sender, DrawingProperties properties)
        {
            sender.EditingMode = InkCanvasEditingMode.Ink;
            Console.WriteLine("Pen drawing   " + properties.Position);
        }

        public PenTool()
        {
            ToolType = ToolType.Pen;
        }
    }
}