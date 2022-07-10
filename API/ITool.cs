using System.Windows.Controls;
using onscreen.Controls;

namespace onscreen.API
{
    public interface ITool
    {
        ToolType ToolType { get; set; }
        
        Control ProcessCreating(DrawingCanvas canvas, DrawingProperties properties);
        
        void ProcessResizing(DrawingCanvas canvas, DrawingProperties properties);
    }
}