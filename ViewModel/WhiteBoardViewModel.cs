using System;

namespace onscreen.ViewModel
{
    public class WhiteBoardViewModel : BaseViewModel
    {
        private Tool _currentTool;
        
        public Tool CurrentTool
        {
            get => _currentTool;
            set
            {
                _currentTool = value;
                OnPropertyChanged(nameof(CurrentTool));
            }
        }
        
    }
}