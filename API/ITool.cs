using System.Windows.Controls;
using onscreen.Controls;

namespace onscreen.API
{
    public interface ITool
    {
        ToolType ToolType { get; set; }

        void Initialize(DrawingCanvas canvas);
        
        Control ProcessCreating(DrawingCanvas canvas, DrawingProperties properties);
        
        void ProcessResizing(DrawingCanvas canvas, DrawingProperties properties);
    }
}