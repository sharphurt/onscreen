using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paint.UndoRedo.Model
{
    public interface IMemento
    {
        object State { get; set; }
    }
}
