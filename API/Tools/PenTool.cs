using System;
using System.Windows.Controls;
using onscreen.Controls;

namespace onscreen.API.Tools
{
    public class PenTool : ITool
    {
        public ToolType ToolType { get; set; }

        public Control ProcessCreating(DrawingCanvas canvas, DrawingProperties properties)
        {
            canvas.EditingMode = InkCanvasEditingMode.Ink;
            return null;
        }

        public void ProcessResizing(DrawingCanvas canvas, DrawingProperties properties)
        {
            
        }

        public PenTool()
        {
            ToolType = ToolType.Pen;
        }
    }
}