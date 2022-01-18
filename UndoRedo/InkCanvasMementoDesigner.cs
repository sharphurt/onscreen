using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paint.UndoRedo.Model;
using System.Windows.Controls;
using System.Windows.Ink;

namespace Paint.UndoRedo
{
    public class InkCanvasMementoDesigner : IMementoDesigner
    {
        readonly InkCanvas _inkCanvas;

        public InkCanvasMementoDesigner(InkCanvas inkCanvas)
        {
            _inkCanvas = inkCanvas;
        }

        public IMemento CreateMemento()
        {
            var copy = _inkCanvas.Strokes.ToArray();
            return new StrokesMemento() { State = copy };
        }

        public void SetMemento(IMemento memento)
        {
            _inkCanvas.Strokes = new StrokeCollection((Stroke[])memento.State);
        }
    }
}
