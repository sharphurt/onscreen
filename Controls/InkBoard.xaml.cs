using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace onscreen.Controls;

public partial class InkBoard : InkCanvas
{
    private Dictionary<int, int> _penWidthChangeStep = new Dictionary<int, int>()
    {
        { 20, 1 },
        { 50, 5 },
        { 100, 10 },
        { 250, 50 },
        { 500, 100 }
    };

    private DrawingAttributes _drawingAttributes = new DrawingAttributes
    {
        Color = Color.FromRgb(255, 255, 255), Width = 2, Height = 2, FitToCurve = true
    };

    public InkBoard()
    {
        InitializeComponent();

        DefaultDrawingAttributes = _drawingAttributes;
    }

    private int GetStepFromWidth(int width)
    {
        int step = 1;

        foreach (var key in _penWidthChangeStep.Keys.Where(key => width >= key))
        {
            step = _penWidthChangeStep[key];
        }

        return step;
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        ChangePenSize(e.Delta > 0);

        DefaultDrawingAttributes = _drawingAttributes;
    }

    private void ChangePenSize(bool direction)
    {
        if ((_drawingAttributes.Width <= 1 && !direction) || (_drawingAttributes.Width >= 500 && direction))
            return;

        var step = GetStepFromWidth((int)_drawingAttributes.Width) * (direction ? 1 : -1);

        _drawingAttributes.Width += step;
        _drawingAttributes.Height += step;
    }
}