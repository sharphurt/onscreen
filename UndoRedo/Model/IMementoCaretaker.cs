using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paint.UndoRedo.Model
{
    public interface IMementoCaretaker
    {
        void Initialize();
        void StoreState();
        void Undo();
    }
}
