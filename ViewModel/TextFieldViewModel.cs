using System.ComponentModel;

namespace onscreen.ViewModel;

public class TextFieldViewModel : BaseViewModel
{
    private string _text;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged(nameof(Text));
        }
    }

    private bool _isBold;

    public bool IsBold
    {
        get => _isBold;
        set
        {
            _isBold = value;
            OnPropertyChanged(nameof(IsBold));
        }
    }
    
    private bool _isItalic;

    public bool IsItalic
    {
        get => _isItalic;
        set
        {
            _isItalic = value;
            OnPropertyChanged(nameof(IsItalic));
        }
    }
    
    private bool _isUnderline;

    public bool IsUnderline
    {
        get => _isUnderline;
        set
        {
            _isUnderline = value;
            OnPropertyChanged(nameof(IsUnderline));
        }
    }
}