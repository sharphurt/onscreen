namespace onscreen.UndoRedo.Model
{
    public interface IMementoCaretaker
    {
        void Initialize();
        void StoreState();
        void Undo();
    }
}
