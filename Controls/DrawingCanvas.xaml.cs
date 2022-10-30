using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using onscreen.API;
using onscreen.API.Tools;
using onscreen.UndoRedo;
using onscreen.UndoRedo.Model;
using Paint.UndoRedo;
using Color = System.Windows.Media.Color;

namespace onscreen.Controls
{
    public partial class DrawingCanvas : Canvas
    {
        private readonly IMementoCaretaker _undoRedoCaretaker;

        public ITool CurrentTool
        {
            get { return (ITool)GetValue(CurrentToolProperty); }
            set
            {
                SetValue(CurrentToolProperty, value);
                SetTextLayerActivity(value.ToolType == ToolType.Text);
            }
        }

        public static readonly DependencyProperty CurrentToolProperty =
            DependencyProperty.Register("CurrentTool", typeof(ITool), typeof(DrawingCanvas));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == CurrentToolProperty)
            {
                CurrentTool.Initialize(this);
                RemoveAllEmptyControls(this);
            }

            base.OnPropertyChanged(e);
        }

        private readonly Dictionary<int, int> _penWidthChangeStep = new Dictionary<int, int>
        {
            { 20, 1 },
            { 50, 5 },
            { 100, 10 },
            { 250, 50 },
            { 500, 100 }
        };
        
        private readonly DrawingAttributes _drawingAttributes = new DrawingAttributes
        {
            Color = Color.FromRgb(255, 255, 255), Width = 2, Height = 2, FitToCurve = true
        };
        
        private bool _isActiveDrawing;

        private Control _lastCreatedControl;

        private bool _isEraseByStroke
        {
            set
            {
                InkCanvas.EditingMode = value switch
                {
                    true when InkCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint => InkCanvasEditingMode.EraseByStroke,
                    false when InkCanvas.EditingMode == InkCanvasEditingMode.EraseByStroke => InkCanvasEditingMode.EraseByPoint,
                    _ => InkCanvas.EditingMode
                };
            }
        }

        public DrawingCanvas()
        {
            InitializeComponent();

            paintWorkspace.MouseUp += DrawingCanvas_OnMouseUp;

            SizeChanged += (_, _) =>
            {
                var left = (ActualWidth - ToolsPanel.ActualWidth) / 2;
                var top = (ActualHeight - ToolsPanel.ActualHeight);

                ToolsPanel.Margin = new Thickness(left, top, 0, 0);
            };

            ColorPalette.OnPaletteChangingColor += OnPalleteColorChange;
            
            InkCanvas.DefaultDrawingAttributes = _drawingAttributes;
            
            var mementoDesigner = new InkCanvasMementoDesigner(InkCanvas);
            _undoRedoCaretaker = new UndoRedoCaretaker(mementoDesigner);
            _undoRedoCaretaker.Initialize();
        }
        
        void OnPalleteColorChange(Color color)
        {
            _drawingAttributes.Color = color;
            InkCanvas.DefaultDrawingAttributes = _drawingAttributes;
        }
        
        private int GetStepFromWidth(int width)
        {
            var step = 1;

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

            InkCanvas.DefaultDrawingAttributes = _drawingAttributes;
        }

        private void ChangePenSize(bool direction)
        {
            if ((_drawingAttributes.Width <= 1 && !direction) || (_drawingAttributes.Width >= 500 && direction))
                return;

            var step = GetStepFromWidth((int)_drawingAttributes.Width) * (direction ? 1 : -1);

            _drawingAttributes.Width += step;
            _drawingAttributes.Height += step;

            SetEraserSize(_drawingAttributes.Width);
        }

        private void SetEraserSize(double size)
        {
            var currentEditingMode = InkCanvas.EditingMode;
            var lastColor = _drawingAttributes.Color;
            
            InkCanvas.EraserShape = new EllipseStylusShape(size, size);
            InkCanvas.DefaultDrawingAttributes.Color = Colors.White;
            InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            
            InkCanvas.EditingMode = currentEditingMode;
            InkCanvas.DefaultDrawingAttributes.Color = lastColor;
        }
        
        private void DrawingCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _undoRedoCaretaker.StoreState();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.LeftCtrl && e.IsDown)
            {
                _isEraseByStroke = true;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == Key.LeftCtrl && e.IsUp)
            {
                _isEraseByStroke = false;
            }
        }

        public void OnExecutedCommands(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Undo)
            {
                _undoRedoCaretaker.Undo();
            }
        }

        private void DrawingCanvas_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Util.IsClickedToExistingControl(this, e.GetPosition(this)))
                RemoveAllEmptyControls(this);

            if (CurrentTool.ToolType != ToolType.Pen && !Util.IsClickedToExistingControl(this, e.GetPosition(this)))
            {
                _lastCreatedControl =
                    CurrentTool.ProcessCreating(this, new DrawingProperties { Position = e.GetPosition(this) });
                _isActiveDrawing = true;
                e.Handled = true;
            }
        }

        private void DrawingCanvas_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _lastCreatedControl != null && _isActiveDrawing)
            {
                CurrentTool.ProcessResizing(this, new DrawingProperties
                {
                    Position = e.GetPosition(this),
                    Control = _lastCreatedControl
                });

                e.Handled = true;
            }
        }

        private void DrawingCanvas_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isActiveDrawing = false;
        }

        private void RemoveAllEmptyControls(DrawingCanvas canvas)
        {
            TextTool.RemoveAllEmpty(canvas);
        }

        private void EraserToolButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new PenTool();
            
            _drawingAttributes.IsHighlighter = false;
            InkCanvas.DefaultDrawingAttributes = _drawingAttributes;
            InkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            InkCanvas.EraserShape = new EllipseStylusShape(_drawingAttributes.Width, _drawingAttributes.Height);
        }

        private void PenToolButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new PenTool();
            
            _drawingAttributes.IsHighlighter = false;
            InkCanvas.DefaultDrawingAttributes = _drawingAttributes;
            InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void HighlighterToolButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new PenTool();

            _drawingAttributes.IsHighlighter = true;
            InkCanvas.DefaultDrawingAttributes = _drawingAttributes;
            InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }
        
        private void TextToolButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new TextTool();
            
            InkCanvas.EditingMode = InkCanvasEditingMode.None;
        }
        
        private void SelectButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new PenTool();

            InkCanvas.EditingMode = InkCanvasEditingMode.Select;
        }

        private void SetTextLayerActivity(bool isActive)
        {
            foreach (var textField in Children.OfType<TextField>())
            {
                textField.IsEnabled = isActive;
                textField.IsHitTestVisible = isActive;
            }
        }
    }
}