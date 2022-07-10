using onscreen.Controls;

namespace onscreen.API
{
    public interface ITool
    {
        ToolType ToolType { get; set; }
        
        void Process(DrawingCanvas canvas, DrawingProperties properties);
    }
}