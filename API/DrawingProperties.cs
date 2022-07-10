using System.Windows;

namespace onscreen.API
{
    public class DrawingProperties
    {
        public Point Position { get; }

        public DrawingProperties(Point position)
        {
            Position = position;
        }
    }
}