using Paint.UndoRedo.Model;

namespace onscreen.UndoRedo
{
    public class StrokesMemento : IMemento
    {
        public object State { get; set; }
    }
}
