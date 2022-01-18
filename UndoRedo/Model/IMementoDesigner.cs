using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paint.UndoRedo.Model
{
    public interface IMementoDesigner
    {
        IMemento CreateMemento();
        void SetMemento(IMemento memento);
    }
}
