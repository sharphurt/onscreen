using System;
using System.Windows.Controls;
using onscreen.Controls;

namespace onscreen.API.Tools
{
    public class PenTool : ITool
    {
        public ToolType ToolType { get; set; }

        public void Process(DrawingCanvas canvas, DrawingProperties properties)
        {
            canvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        public PenTool()
        {
            ToolType = ToolType.Pen;
        }
    }
}