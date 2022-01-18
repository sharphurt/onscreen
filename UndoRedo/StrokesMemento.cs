using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paint.UndoRedo.Model;
using System.Windows.Ink;

namespace Paint.UndoRedo
{
    public class StrokesMemento : IMemento
    {
        public object State { get; set; }
    }
}
