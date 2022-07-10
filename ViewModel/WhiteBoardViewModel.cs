using System;
using System.Windows.Input;
using onscreen.API;
using onscreen.API.Tools;

namespace onscreen.ViewModel
{
    public class WhiteBoardViewModel : BaseViewModel
    {
        private ITool _currentTool;

        public ITool CurrentTool
        {
            get => _currentTool;
            set
            {
                _currentTool = value;
                OnPropertyChanged(nameof(CurrentTool));
            }
        }

        public ICommand SelectToolCommand { get; set; }

        public WhiteBoardViewModel()
        {
            CurrentTool = new PenTool();
            SelectToolCommand = new RelayCommand(SelectTool, CanSelectTool);
        }

        private void SelectTool(object o)
        {
            var type = (ToolType)o;

            switch (type)
            {
                case ToolType.Pen:
                    CurrentTool = new PenTool();
                    break;
                case ToolType.Text:
                    CurrentTool = new TextTool();
                    break;
            }
        }

        private bool CanSelectTool(object o)
        {
            return true;
        }
    }
}