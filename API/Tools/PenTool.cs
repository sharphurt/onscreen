using System;
using System.Windows.Controls;
using onscreen.Controls;

namespace onscreen.API.Tools
{
    public class PenTool : ITool
    {
        public ToolType ToolType { get; set; }

        public void Initialize(DrawingCanvas canvas)
        {
            canvas.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        public Control ProcessCreating(DrawingCanvas canvas, DrawingProperties properties)
        {
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